using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class SingleTargeting(SingleTargeting.Type type) : ITargetingComponent
{
    private readonly Func<AbilityExecContext, IEnumerable<TargetingContext>> _selector = type switch
    {
        Type.Body => SelectBody,
        Type.WeakPoint => SelectWeakPoint,
        Type.Upswing => SelectSlam,
        Type.Downswing => SelectFling,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
    };

    IEnumerable<TargetingContext> ITargetingComponent.SelectTargets(AbilityExecContext ctx) => _selector(ctx);

    private static IEnumerable<TargetingContext> SelectBody(AbilityExecContext ctx)
    {
        return
        [
            new TargetingContext
            {
                Target = ctx.MainTarget.Parts[TargetTag.MainBody],
                TargetRank = 0,
            }
        ];
    }

    private static IEnumerable<TargetingContext> SelectWeakPoint(AbilityExecContext ctx)
    {
        return
        [
            new TargetingContext
            {
                Target = ctx.MainTarget.Parts[TargetTag.WeakPoint],
                TargetRank = 0
            }
        ];
    }

    private static IEnumerable<TargetingContext> SelectSlam(AbilityExecContext ctx)
    {
        return PartTargeting.Slam(ctx.MainTarget, 0, true);
    }

    private static IEnumerable<TargetingContext> SelectFling(AbilityExecContext ctx)
    {
        return PartTargeting.Slam(ctx.MainTarget, 0);
    }

    public enum Type
    {
        Body,
        WeakPoint,
        Upswing,
        Downswing,
    }
}