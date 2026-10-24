using System.Linq;
using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Modifiers;

/// <summary>
/// Represents a smarter ModifierStack that determines to which kind of queries to apply.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(StatModifier), "stat")]
[JsonDerivedType(typeof(DamageModifier), "damage")]
[JsonDerivedType(typeof(PoolMutModifier), "mutation")]
[JsonDerivedType(typeof(PoolStatModifier), "pool_property")]
[JsonDerivedType(typeof(DamageStatModifier), "damage_property")]
public interface IModifierRule
{
    ModifierStack ModifierStack { get; }

    /// <summary>
    /// Determines whether this modifier applies to the given query.
    /// </summary>
    /// <param name="query">Query to check application for.</param>
    /// <param name="ctx">Data containing the modifier holder and the subject we're calculating for.</param>
    /// <returns>True if this modifier applies to the query in this context, false otherwise.</returns>
    bool AppliesTo(IQuery query, in ModifierContext ctx);
}

/// <summary>
/// Wraps a ModifierStack and determines to which kinds of queries it applies.
/// <br/>Base generic class for common code.
/// </summary>
/// <typeparam name="TQuery">Type of the query this modifier applies to.</typeparam>
public abstract class ModifierRule<TQuery> : IModifierRule where TQuery : IQuery
{
    public required ModifierStack ModifierStack { get; init; }

    bool IModifierRule.AppliesTo(IQuery query, in ModifierContext ctx)
    {
        return query is TQuery q && MatchesRelationship(q, ctx) && AppliesTo(q);
    }

    protected abstract bool AppliesTo(TQuery query);
    protected abstract bool MatchesRelationship(TQuery query, in ModifierContext ctx);
}

/// <summary>
/// Modifier that only affects values inherent to a single entity.
/// <br/>Inherit for non-mutation-type modifiers.
/// </summary>
/// <typeparam name="TQuery">Type of the query this modifier applies to.</typeparam>
public abstract class AttributeModifier<TQuery> : ModifierRule<TQuery> where TQuery : AttributeQuery
{
    /// <summary>
    /// Determines whether this modifier applies to the holder or the target.
    /// </summary>
    public required AffectedSide AffectedSide { get; init; }

    protected sealed override bool MatchesRelationship(TQuery query, in ModifierContext ctx)
    {
        var subject = query.Subject;
        var reference = query.Reference;
        return AffectedSide switch
        {
            AffectedSide.This => ctx.Holder == subject,
            AffectedSide.Other => ctx.Holder == reference,
            _ => throw new System.NotSupportedException($"{AffectedSide} not supported")
        } && ctx.Subject == subject;
    }
}

/// <summary>
/// Modifier that affects values meaningful in a relation between
/// two entities, such as damage dealt, or mana restored by another character.
/// <br/>Inherit for mutation-type modifiers.
/// </summary>
/// <typeparam name="TQuery">Type of the query this modifier applies to.</typeparam>
public abstract class MutationModifier<TQuery> : ModifierRule<TQuery> where TQuery : CommandQuery
{
    /// <summary>
    /// Determines whether this modifier applies to the holder or the target.
    /// </summary>
    public required AffectedSide AffectedSide { get; init; }
    /// <summary>
    /// Determines whether this modifier applies to incoming or outgoing mutations.
    /// </summary>
    public required MutationDirection Direction { get; init; }

    protected sealed override bool MatchesRelationship(TQuery query, in ModifierContext ctx)
    {
        var (affected, other) = Direction switch
        {
            // Outgoing-type mods modify actor's stats
            MutationDirection.Outgoing => (query.Context.Actor, query.Context.Target),
            // Incoming-type mods modify target's stats
            MutationDirection.Incoming => (query.Context.Target, query.Context.Actor),
            _ => throw new System.NotSupportedException($"{Direction} not supported")
        };
        var expectedHolder = AffectedSide switch
        {
            // Holder's stats are modified
            AffectedSide.This => affected,
            // Other's stats are modified
            AffectedSide.Other => other,
            _ => throw new System.NotSupportedException($"{AffectedSide} not supported")
        };

        return ctx.Subject == affected && ctx.Holder == expectedHolder;
    }

    [System.Obsolete] // Kept here because I like the switch. Also, it explains each case better.
    private bool MatchesRelationshipSwitch(CommandQuery query, in ModifierContext ctx)
    {
        var actor = query.Context.Actor;
        var target = query.Context.Target;
        return (AffectedSide, Direction) switch
        {
            (AffectedSide.This, MutationDirection.Outgoing) // Attacker's outgoing modifiers
                => ctx.Holder == actor && ctx.Subject == actor,
            (AffectedSide.Other, MutationDirection.Outgoing) // Target's outgoing penalty modifiers
                => ctx.Holder == target && ctx.Subject == actor,
            (AffectedSide.This, MutationDirection.Incoming) // Target's incoming modifiers
                => ctx.Holder == target && ctx.Subject == target,
            (AffectedSide.Other, MutationDirection.Incoming) // Attacker's incoming penalty modifiers
                => ctx.Holder == actor && ctx.Subject == target,

            _ => throw new System.NotSupportedException($"Combination of {AffectedSide} and {Direction} not supported")
        };
    }
}

/// <summary>
/// Context for modifier application.
/// </summary>
/// <param name="Holder">Holder of the modifier.</param>
/// <param name="Subject">Entity whose value is being modified.</param>
public readonly record struct ModifierContext(EntityUid Holder, EntityUid Subject);

/// <summary>
/// Represents which character's stats get modified (holder or target)
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<AffectedSide>))]
public enum AffectedSide
{
    /// <summary>
    /// The effects of this modifier will affect the holder. (e.g. holder's DamageRes)
    /// </summary>
    [JsonStringEnumMemberName("this")]
    This,
    /// <summary>
    /// The effects of this modifier will affect the other. (e.g. holder's DamageRes PENALTY)
    /// </summary>
    [JsonStringEnumMemberName("other")]
    Other
}

/// <summary>
/// Represents the direction of a mutation (incoming or outgoing)
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter<MutationDirection>))]
public enum MutationDirection
{
    /// <summary>
    /// Modifies the mutation as it leaves Actor. (e.g. holder's damage DEALT)
    /// </summary>
    [JsonStringEnumMemberName("outgoing")]
    Outgoing,
    /// <summary>
    /// Modifies the mutation as it affects Target. (e.g. holder's damage TAKEN)
    /// </summary>
    [JsonStringEnumMemberName("incoming")]
    Incoming,
}

/// <summary>
/// Defines methods for aggregating collections of IModifierRules into one ModifierStack.
/// </summary>
public static class ModifierRules
{
    public static ModifierStack Aggregate(System.Collections.Generic.IEnumerable<IModifierRule> rules, IQuery query, ModifierContext ctx)
    {
        return rules.Where(rule => rule.AppliesTo(query, ctx)).Select(rule => rule.ModifierStack)
            .Aggregate((current, next) => current + next);
    }
}