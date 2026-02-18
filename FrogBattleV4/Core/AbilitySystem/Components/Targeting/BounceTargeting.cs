using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class BounceTargeting(TargetingType type) : ITargetingComponent
{
    public required int Count { get; init; }

    public IEnumerable<AbilityTargetingContext> SelectTargets(AbilityExecContext ctx)
    {
        var targets = Enumerable.Empty<IBattleMember>();
        for (var i = 0; i < Count; i++)
        {
            targets = targets.Append(ctx.ValidTargets.MinBy(_ => ctx.Rng.NextDouble()));
        }

        return targets.Select(bm => new AbilityTargetingContext
        {
            Target = bm,
            Aiming = type,
            Rank = 1,
        }).Prepend(new AbilityTargetingContext
        {
            Target = ctx.MainTarget,
            Aiming = type,
            Rank = 0,
        });
    }
}