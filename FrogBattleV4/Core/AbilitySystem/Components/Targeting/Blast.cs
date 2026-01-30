#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class Blast : ITargetingComponent
{
    public uint Radius { get; init; } = 0;

    public IEnumerable<TargetingContext> SelectTargets(AbilityExecContext ctx)
    {
        IEnumerable<TargetingContext> result = [new()
        {
            Target = ctx.MainTarget,
            TargetRank = 0
        }];
        var idx = ctx.ValidTargets.IndexOf(ctx.MainTarget);
        if (idx == -1) throw new InvalidOperationException("MainTarget is not among ValidTargets");
        for (var rank = 1; idx + rank < ctx.ValidTargets.Count && rank <= Radius; rank++)
        {
            result = result.Append(new TargetingContext
            {
                Target = ctx.ValidTargets[idx + rank],
                TargetRank = rank
            });
        }
        for (var rank = 1; idx - rank >= 0 && rank <= Radius; rank++)
        {
            result = result.Append(new TargetingContext
            {
                Target = ctx.ValidTargets[idx - rank],
                TargetRank = rank
            });
        }
        return result;
    }
}