using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

/// <summary>
/// Helper class for targeting logic.
/// </summary>
internal static class HitboxTargeting
{
    [Pure]
    public static IEnumerable<AbilityTargetingContext> Select(TargetingType type, BattleMember target, int rank)
    {
        target.Hitbox.Resolve(type).Deconstruct(out var isHit, out var mods);
        if (isHit)
            yield return new AbilityTargetingContext
            {
                Modifiers = mods,
                Target = target,
                TargetRank = rank,
            };
    }

    [Pure]
    public static IEnumerable<AbilityTargetingContext> SelectAll(TargetingType type, IEnumerable<BattleMember> targets,
        System.Func<int, int> rankSelector)
    {
        foreach (var (index, target) in targets.Index())
        {
            target.Hitbox.Resolve(type).Deconstruct(out var isHit, out var mods);
            if (isHit)
                yield return new AbilityTargetingContext
                {
                    Modifiers = mods,
                    Target = target,
                    TargetRank = rankSelector(index),
                };
        }
    }
}

/// <summary>
/// Custom enum with a payload.
/// </summary>
public abstract record TargetingType
{
    public sealed record Region(HitboxRegion Value) : TargetingType;

    public sealed record Height(int Value) : TargetingType;

    public static readonly TargetingType Body = new Region(HitboxRegion.Body);
    public static readonly TargetingType WeakPoint = new Region(HitboxRegion.WeakPoint);
    public static readonly TargetingType Ground = new Height(0);
}