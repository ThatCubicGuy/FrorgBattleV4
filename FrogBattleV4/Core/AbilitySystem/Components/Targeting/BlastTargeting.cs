#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class BlastTargeting(TargetingType type) : ITargetingComponent
{
    public required int Radius { get; init; }

    public IEnumerable<AbilityTargetingContext> SelectTargets(AbilityExecContext ctx)
    {
        var validTargetsList = ctx.ValidTargets.ToList();
        var idx = validTargetsList.IndexOf(ctx.MainTarget);
        if (idx == -1) throw new InvalidOperationException("MainTarget is not among ValidTargets");

        // Project each target in the list into an AbilityTargetingContext with rank being the distance
        // of this target from the main target (horizontally). Then, skip anything below idx-Radius and
        // take every element within the radius from there.
        return validTargetsList.Select((bm, i) => new AbilityTargetingContext
        {
            Target = bm,
            Aiming = type,
            Rank = Math.Abs(idx - i),
        }).Skip(idx - Radius).Take(2 * Radius + 1);
    }
}