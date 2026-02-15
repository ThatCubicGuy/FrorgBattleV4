using System.Collections.Generic;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class AreaOfEffectTargeting(TargetingType type) : ITargetingComponent
{
    public IEnumerable<AbilityTargetingContext> SelectTargets(AbilityExecContext ctx) =>
        HitboxTargeting.SelectAll(type, ctx.ValidTargets, _ => 0);
}