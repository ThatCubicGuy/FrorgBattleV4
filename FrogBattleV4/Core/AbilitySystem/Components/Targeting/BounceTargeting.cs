using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class BounceTargeting : ITargetingComponent
{
    public required int Count { get; init; }

    public IEnumerable<TargetingContext> SelectTargets(AbilityExecContext ctx)
    {
        List<TargetingContext> result = [PartTargeting.ByTag(ctx.MainTarget, 0, TargetTag.MainBody)];
        for (var i = 0; i < Count; i++)
        {
            result.Add(new TargetingContext
            {
                Target = ctx.ValidTargets.MinBy(_ => ctx.Rng.NextDouble()),
                TargetRank = 1,
            });
        }
        return result;
    }
}