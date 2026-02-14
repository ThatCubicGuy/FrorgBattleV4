using System;
using System.Collections.Generic;
using System.ComponentModel;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class SingleTargeting(SingleTargeting.SingleType type) : ITargetingComponent
{
    public IEnumerable<TargetingContext> SelectTargets(AbilityExecContext ctx) => type switch
    {
        SingleType.Body => PartTargeting.ByTag(ctx.MainTarget, 0, TargetTag.MainBody),
        SingleType.WeakPoint => PartTargeting.ByTag(ctx.MainTarget, 0, TargetTag.WeakPoint),
        SingleType.Downswing => PartTargeting.Vertical(ctx.MainTarget, 0),
        SingleType.Upswing => PartTargeting.Vertical(ctx.MainTarget, 0, true),
        _ => throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(SingleType))
    };

    public enum SingleType
    {
        Body,
        WeakPoint,
        Downswing,
        Upswing,
    }
}