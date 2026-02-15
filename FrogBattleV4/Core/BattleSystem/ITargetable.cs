#nullable enable
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.AbilitySystem.Components.Targeting;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.BattleSystem;

public interface ITargetable
{
    /// <summary>
    /// Resolve a selection for this type of targeting.
    /// </summary>
    /// <param name="targeting"></param>
    /// <returns></returns>
    [Pure]
    TargetingResult Resolve(TargetingType targeting);
}

public readonly record struct TargetingResult(bool IsHit, IEnumerable<IModifierRule> Modifiers)
{
    public static readonly TargetingResult Miss = new() { IsHit = false };
}

public enum HitboxRegion
{
    Body,
    WeakPoint
}