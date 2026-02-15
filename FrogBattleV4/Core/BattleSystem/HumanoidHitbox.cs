#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.AbilitySystem.Components.Targeting;
using FrogBattleV4.Core.EffectSystem.Components;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.BattleSystem;

public class HumanoidHitbox : ITargetable
{
    /// <summary>
    /// If the character is floating, they will be immune to ground attacks.
    /// </summary>
    public bool Floating { get; set; }

    public required IEnumerable<DamageModifier> HeadshotModifiers { get; init; }

    public IEnumerable<DamageModifier> NormalModifiers { get; init; } = [];

    [Pure]
    private HitboxRegion? GetRegionAtHeight(int height) => height switch
    {
        0 => Floating ? null : HitboxRegion.Body,
        1 => HitboxRegion.Body,
        2 => HitboxRegion.WeakPoint,
        _ => null
    };

    [Pure]
    private IEnumerable<IModifierRule> GetModifiers(HitboxRegion region) => region switch
    {
        HitboxRegion.Body => NormalModifiers,
        HitboxRegion.WeakPoint => HeadshotModifiers,
        _ => []
    };

    public TargetingResult Resolve(TargetingType targeting)
    {
        return targeting switch
        {
            TargetingType.Region r => new TargetingResult(true, GetModifiers(r.Value)),
            TargetingType.Height h => GetRegionAtHeight(h.Value) is not { } region
                ? TargetingResult.Miss
                : new TargetingResult(true, GetModifiers(region)),
            _ => throw new NotSupportedException("Unknown targeting type: " + targeting)
        };
    }
}