using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

/// <summary>
/// Represents a smarter ModifierStack that determines to which kind of queries to apply.
/// <br/>Base non-generic class used to hold collections.
/// </summary>
public abstract class ModifierRule
{
    public required ModifierStack ModifierStack { get; init; }

    /// <summary>
    /// Determines whether this modifier applies to the given query.
    /// </summary>
    /// <param name="query">Query to check application for.</param>
    /// <param name="ctx">Data containing the modifier holder and the subject we're calculating for.</param>
    /// <returns></returns>
    public abstract bool Applies(IQuery query, in ModifierContext ctx);
}

/// <summary>
/// Wraps a ModifierStack and determines to which kinds of queries it applies.
/// <br/>Base generic class for common code.
/// </summary>
/// <typeparam name="TQuery">Type of the query this modifier applies to.</typeparam>
public abstract class ModifierRule<TQuery> : ModifierRule where TQuery : IQuery
{
    public sealed override bool Applies(IQuery query, in ModifierContext ctx)
    {
        return query is TQuery q && MatchesRelationship(q, ctx) && AppliesTo(q);
    }

    protected abstract bool AppliesTo(TQuery query);
    protected abstract bool MatchesRelationship(TQuery query, in ModifierContext ctx);
}

/// <summary>
/// Stat modifier that only affects values inherent to a single entity.
/// <br/>Inherit for non-mutation-type modifiers.
/// </summary>
/// <typeparam name="TQuery">Type of the query this modifier applies to.</typeparam>
public abstract class StatModifier<TQuery> : ModifierRule<TQuery> where TQuery : StaticQuery
{
    public required AffectedSide AffectedSide { get; init; }

    protected sealed override bool MatchesRelationship(TQuery query, in ModifierContext ctx)
    {
        var actor = query.Subject;
        var target = query.Reference;
        return AffectedSide switch
        {
            AffectedSide.Self => ctx.Holder == actor,
            AffectedSide.Other => ctx.Holder == target,
            _ => throw new System.NotSupportedException($"{AffectedSide} not supported")
        } && ctx.Subject == actor;
    }
}

/// <summary>
/// Mutation modifier that affects values meaningful in a relation between
/// two entities, such as damage dealt, or mana restored by another character.
/// <br/>Inherit for mutation-type modifiers.
/// </summary>
/// <typeparam name="TQuery">Type of the query this modifier applies to.</typeparam>
public abstract class MutationModifier<TQuery> : ModifierRule<TQuery> where TQuery : DynamicQuery
{
    public required AffectedSide AffectedSide { get; init; }
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
            AffectedSide.Self => affected,
            // Other's stats are modified
            AffectedSide.Other => other,
            _ => throw new System.NotSupportedException($"{AffectedSide} not supported")
        };

        return ctx.Subject == affected && ctx.Holder == expectedHolder;
    }

    [System.Obsolete] // Kept here because I like the switch. Also, it explains each case better.
    private bool MatchesRelationshipSwitch(DynamicQuery query, in ModifierContext ctx)
    {
        var actor = query.Context.Actor;
        var target = query.Context.Target;
        return (AffectedSide, Direction) switch
        {
            (AffectedSide.Self, MutationDirection.Outgoing) // Attacker's outgoing modifiers
                => ctx.Holder == actor && ctx.Subject == actor,
            (AffectedSide.Other, MutationDirection.Outgoing) // Target's outgoing penalty modifiers
                => ctx.Holder == target && ctx.Subject == actor,
            (AffectedSide.Self, MutationDirection.Incoming) // Target's incoming modifiers
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
public enum AffectedSide
{
    /// <summary>
    /// The effects of this modifier will affect the holder. (e.g. holder's DamageRes)
    /// </summary>
    Self,
    /// <summary>
    /// The effects of this modifier will affect the other. (e.g. holder's DamageRes PENALTY)
    /// </summary>
    Other
}

/// <summary>
/// Represents the direction of a mutation (incoming or outgoing)
/// </summary>
public enum MutationDirection
{
    /// <summary>
    /// Modifies the mutation as it leaves Actor. (e.g. holder's damage DEALT)
    /// </summary>
    Outgoing,
    /// <summary>
    /// Modifies the mutation as it affects Target. (e.g. holder's damage TAKEN)
    /// </summary>
    Incoming,
}