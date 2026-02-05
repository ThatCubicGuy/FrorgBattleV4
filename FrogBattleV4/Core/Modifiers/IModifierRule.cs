#nullable enable

using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Modifiers;

public interface IModifierRule
{
    ModifierStack ModifierStack { get; }
    /// <summary>
    /// Determines whether this modifier applies for this query.
    /// </summary>
    /// <param name="query"></param>
    /// <returns>True if the modifier applies, false otherwise.</returns>
    [Pure]
    bool AppliesFor(object query);
}

public interface IModifierRule<in TQuery> : IModifierRule where TQuery : struct
{
    [Pure]
    bool AppliesFor(TQuery query);

    [Pure]
    bool IModifierRule.AppliesFor(object query) => query switch
    {
        TQuery typedQuery => AppliesFor(typedQuery),
        _ => false
    };
}

public enum ModifierDirection
{
    Incoming,
    Outgoing
}