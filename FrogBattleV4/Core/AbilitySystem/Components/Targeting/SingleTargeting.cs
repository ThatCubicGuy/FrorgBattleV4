using System.Collections.Generic;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class SingleTargeting(TargetingType type) : ITargetingComponent
{
    public IEnumerable<AbilityTargetingContext> SelectTargets(AbilityExecContext ctx) =>
        HitboxTargeting.Select(type, ctx.MainTarget, 0);
}