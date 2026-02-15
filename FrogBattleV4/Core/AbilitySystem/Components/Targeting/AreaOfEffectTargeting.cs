using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class AreaOfEffectTargeting(TargetingType type) : ITargetingComponent
{
    public IEnumerable<AbilityTargetingContext> SelectTargets(AbilityExecContext ctx)
    {
        return ctx.ValidTargets.Select(bm => new AbilityTargetingContext
        {
            Target = bm,
            Aiming = type,
            Rank = 0,
        });
    }
}