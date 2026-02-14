#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class BlastTargeting(uint radius = 1) : ITargetingComponent
{
    public uint Radius { get; init; } = radius;

    // waaaaaaaa TODO absolutely EVERYTHINGGGGGG RAHHHHH
    public IEnumerable<TargetingContext> SelectTargets(AbilityExecContext ctx)
    {
        IEnumerable<TargetingContext> result = [new()
        {
            Target = ctx.MainTarget,
            TargetRank = 0
        }];
        var validTargetsList = ctx.ValidTargets.ToList();
        var idx = validTargetsList.IndexOf(ctx.MainTarget);
        if (idx == -1) throw new InvalidOperationException("MainTarget is not among ValidTargets");
        for (var rank = 1; idx + rank < validTargetsList.Count && rank <= Radius; rank++)
        {
            result = result.Append(new TargetingContext
            {
                Target = validTargetsList[idx + rank],
                TargetRank = rank
            });
        }
        for (var rank = 1; idx - rank >= 0 && rank <= Radius; rank++)
        {
            result = result.Append(new TargetingContext
            {
                Target = validTargetsList[idx - rank],
                TargetRank = rank
            });
        }
        return result;
    }
}