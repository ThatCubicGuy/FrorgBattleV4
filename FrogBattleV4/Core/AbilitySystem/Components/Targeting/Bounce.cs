using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class Bounce : ITargetingComponent
{
    public required int Count { get; init; }

    public IEnumerable<TargetingContext> SelectTargets(AbilityExecContext ctx)
    {
        List<TargetingContext> result = [new()
        {
            Target = ctx.MainTarget,
            TargetRank = 0
        }];
        result.AddRange(ctx.ValidTargets.OrderBy(x => ctx.Rng.NextDouble()).Select(x => new TargetingContext
        {
            Target = ctx.MainTarget,
            TargetRank = 1
        }).Take(Count));
        return result;
    }
}