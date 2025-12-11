#nullable enable
using System.Collections.Generic;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class Blast : ITargetingComponent
{
    public uint Radius { get; init; } = 0;

    public IEnumerable<TargetingContext> SelectTargets(AbilityContext ctx)
    {
        var index = ctx.ValidTargets.IndexOf(ctx.MainTarget);
        if (index == -1) throw new System.ArgumentException("Main target is not a valid target!", nameof(ctx.MainTarget));
        List<TargetingContext> result = [new()
        {
            Target = ctx.MainTarget,
            TargetRank = 0
        }];
        for (var left = 1; left <= Radius && index - left >= 0; left++)
        {
            result.Add(new TargetingContext
            {
                Target = ctx.ValidTargets[index - left],
                TargetRank = left
            });
        }
        for (var right = 1; right <= Radius && index + right < ctx.ValidTargets.Count; right++)
        {
            result.Add(new TargetingContext
            {
                Target = ctx.ValidTargets[index + right],
                TargetRank = right
            });
        }
        return result;
    }
}