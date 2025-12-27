#nullable enable
using System.Collections.Generic;
using System.Linq;

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
        var idx = 0;
        for (var left = ctx.MainTarget.Left; left is not null; left = left.Left)
        {
            idx += 1;
            result = result.Append(new TargetingContext
            {
                Target = left,
                TargetRank = idx
            });
        }
        idx = 0;
        for (var right = ctx.MainTarget.Right; right is not null; right = right.Right)
        {
            idx += 1;
            result = result.Append(new TargetingContext
            {
                Target = right,
                TargetRank = idx
            });
        }
        return result;
    }
}