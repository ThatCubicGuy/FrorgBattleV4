using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class BounceTargeting(TargetingType type) : ITargetingComponent
{
    public required int Count { get; init; }

    public IEnumerable<AbilityTargetingContext> SelectTargets(AbilityExecContext ctx)
    {
        var targets = Enumerable.Empty<BattleMember>()
            .Append(ctx.MainTarget);
        for (var i = 0; i < Count; i++)
        {
            targets = targets.Append(ctx.ValidTargets.MinBy(_ => ctx.Rng.NextDouble()));
        }

        return HitboxTargeting.SelectAll(type, targets, idx => idx == 0 ? 0 : 1);
    }
}