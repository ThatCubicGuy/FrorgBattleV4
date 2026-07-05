#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Effects;
using FrogBattleV4.Core.Effects.Components;
using FrogBattleV4.Core.Effects.Modifiers;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Combat;

public class HumanoidHitbox : IHitbox
{
    /// <summary>
    /// If the character is floating, they will be immune to ground attacks.
    /// </summary>
    public bool Floating { get; set; }

    public required IEnumerable<DamageMutModifier> HeadshotModifiers
    {
        get => throw new NotImplementedException();
        init => throw new NotImplementedException();
    }

    public IEnumerable<DamageMutModifier> DamageModifiers { get; init; } = [];

    [Pure]
    private HitboxRegion? GetRegionAtHeight(int height) => height switch
    {
        0 => Floating ? null : HitboxRegion.Body,
        1 => HitboxRegion.Body,
        2 => HitboxRegion.WeakPoint,
        _ => null
    };

    [Pure]
    private ModifierCollection GetModifiers(HitboxRegion region) => new((region switch
    {
        HitboxRegion.Body => DamageModifiers,
        HitboxRegion.WeakPoint => HeadshotModifiers,
        _ => []
    }).ToArray<ModifierRule>());

    public TargetingResult Resolve(TargetingType targeting)
    {
        return targeting switch
        {
            TargetingType.Region r => new TargetingResult(GetModifiers(r.Value)),
            TargetingType.Height h => GetRegionAtHeight(h.Value) is not { } region
                ? TargetingResult.Miss
                : new TargetingResult(GetModifiers(region)),
            _ => throw new NotSupportedException("Unknown targeting type: " + targeting.GetType().Name)
        };
    }
}