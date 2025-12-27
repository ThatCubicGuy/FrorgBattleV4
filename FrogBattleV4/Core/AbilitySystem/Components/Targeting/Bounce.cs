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
        for (var i = 0; i < Count; i++)
        {
            result.Add(new TargetingContext
            {
                Target = ctx.ValidTargets.MinBy(_ => ctx.Rng.NextDouble()),
                TargetRank = 1
            });
        }
        return result;
    }
}