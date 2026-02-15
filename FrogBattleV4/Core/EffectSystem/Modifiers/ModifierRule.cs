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
    public abstract bool AppliesFor(object query);
}

/// <summary>
/// Directional modifier.
/// </summary>
/// <typeparam name="TRequest">Request type that this modifier handles.</typeparam>
public abstract class ModifierRule<TRequest> : ModifierRule where TRequest : struct
{
    public ModifierDirection Direction { get; init; }
    [Pure]
    protected abstract bool AppliesToRequest(TRequest query);

    [Pure]
    public override bool AppliesFor(object query) => query switch
    {
        ModifierQuery<TRequest> typedQuery => AppliesToRequest(typedQuery.Request) && typedQuery.Direction == Direction,
        _ => false
    };
}

public enum ModifierDirection
{
    /// <summary>
    /// Incoming damage mods, stat mods that affect you,
    /// pool mods that affect incoming mutations.
    /// </summary>
    Self,
    /// <summary>
    /// Outgoing damage mods, stat penalty mods, pool mods
    /// that affect outgoing mutations (what the reference will suffer).
    /// </summary>
    Reference
}