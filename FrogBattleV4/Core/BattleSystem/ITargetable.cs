using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.BattleSystem;

public interface ITargetable
{
    /// <summary>
    /// Resolve a selection for this type of targeting.
    /// </summary>
    /// <param name="targeting"></param>
    /// <returns>The result of being targeted in this way.</returns>
    [Pure]
    TargetingResult Resolve(TargetingType targeting);
}

public readonly record struct TargetingResult([NotNull] IModifierProvider Modifier)
{
    public bool WouldHit { get; private init; } = true;
    public static readonly TargetingResult Miss = new() { WouldHit = false };
}

public enum HitboxRegion
{
    Body,
    WeakPoint
}