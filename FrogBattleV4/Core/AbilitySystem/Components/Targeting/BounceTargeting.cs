using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class BounceTargeting(BounceTargeting.BounceType type) : ITargetingComponent
{
    public required int Count { get; init; }

    public IEnumerable<TargetingContext> SelectTargets(AbilityExecContext ctx) => type switch
    {
        BounceType.Body => SelectTargetsByTag(ctx, TargetTag.MainBody),
        BounceType.WeakPoint => SelectTargetsByTag(ctx, TargetTag.WeakPoint),
        BounceType.SameHeight => 
    };
    public IEnumerable<TargetingContext> SelectTargetsByTag(AbilityExecContext ctx, TargetTag tag)
    {
        var result = PartTargeting.ByTag(ctx.MainTarget, 0, tag);
        for (var i = 0; i < Count; i++)
        {
            result = result.Concat(
                PartTargeting.ByTag(ctx.ValidTargets.MinBy(_ => ctx.Rng.NextDouble()),
                    1,
                    tag));
        }

        return result;
    }

    public IEnumerable<TargetingContext> SelectTargetsBySameHeight(AbilityExecContext ctx)
    {
        var result = PartTargeting.ByTag(ctx.MainTarget, 0, TargetTag.MainBody);
        for (var i = 0; i < Count; i++)
        {
            result = result.Concat(
                PartTargeting.ByTag(ctx.ValidTargets.MinBy(_ => ctx.Rng.NextDouble()),
                    1,
                    tag));
        }

        return result;
    }

    public enum BounceType
    {
        Body,
        WeakPoint,
        SameHeight
    }
}