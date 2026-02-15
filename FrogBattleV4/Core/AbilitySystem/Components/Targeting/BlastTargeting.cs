#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class BlastTargeting(TargetingType type) : ITargetingComponent
{
    public required int Radius { get; init; }

    public IEnumerable<AbilityTargetingContext> SelectTargets(AbilityExecContext ctx)
    {
        var validTargetsList = ctx.ValidTargets.ToList();
        var idx = validTargetsList.IndexOf(ctx.MainTarget);
        if (idx == -1) throw new InvalidOperationException("MainTarget is not among ValidTargets");

        return HitboxTargeting.SelectAll(type, validTargetsList, i => Math.Abs(idx - i))
            .Take(Math.Min(validTargetsList.Count, idx + Radius + 1)).Skip(Math.Max(0, idx - Radius));
    }
}