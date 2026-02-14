using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class AreaOfEffectTargeting(AreaOfEffectTargeting.AoEType type) : ITargetingComponent
{
    public IEnumerable<TargetingContext> SelectTargets(AbilityExecContext ctx) => type switch
    {
        AoEType.Body => ctx.ValidTargets.SelectMany(bm => PartTargeting.ByTag(bm, 0, TargetTag.MainBody)),
        AoEType.WeakPoint => ctx.ValidTargets.SelectMany(bm => PartTargeting.ByTag(bm, 0, TargetTag.WeakPoint)),
        AoEType.All => ctx.ValidTargets.SelectMany(bm => PartTargeting.AllParts(bm, 0)),
        _ => throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(AoEType))
    };

    public enum AoEType
    {
        Body,
        WeakPoint,
        All
    }
}