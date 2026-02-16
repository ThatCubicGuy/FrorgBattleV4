#nullable enable

using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.EffectSystem.Modifiers;

public abstract class ModifierRule
{
    public ModifierStack ModifierStack { get; init; }
    /// <summary>
    /// Determines whether this modifier applies for this query.
    /// </summary>
    /// <param name="query">Query for which to check application.</param>
    /// <returns>True if the modifier applies, false otherwise.</returns>
    [Pure]
    public abstract bool AppliesTo(object query);
}

/// <summary>
/// Modifier that applies only to some queries.
/// </summary>
/// <typeparam name="TQuery">Type of the handled queries.</typeparam>
public abstract class ModifierRule<TQuery> : ModifierRule where TQuery : struct
{
    public ModifierDirection Direction { get; init; } = ModifierDirection.Actor;
    [Pure]
    protected abstract bool AppliesToQuery(TQuery query);

    [Pure]
    public override bool AppliesTo(object query) => query switch
    {
        ModifierQuery<TQuery> typedQuery => AppliesToQuery(typedQuery.Query) && typedQuery.Direction == Direction,
        _ => false
    };
}

public abstract class MutModifierRule<TQuery> : ModifierRule<TQuery> where TQuery : struct
{
    public required MutModifierDirection MutDirection { get; init; }

    [Pure]
    public override bool AppliesTo(object query) => query switch
    {
        MutModifierQuery<TQuery> typedQuery => base.AppliesTo(query) && typedQuery.MutModifierDirection == MutDirection,
        _ => false
    };
}

public enum ModifierDirection
{
    /// <summary>
    /// Things that affect the holder as an actor.
    /// </summary>
    Actor,
    /// <summary>
    /// Things that affect the other as an actor.
    /// </summary>
    Other
}

public enum MutModifierDirection
{
    Outgoing,
    Incoming,
}