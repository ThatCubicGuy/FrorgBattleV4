using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class AreaOfEffectTargeting : ITargetingComponent
{
    public IEnumerable<TargetingContext> SelectTargets(AbilityExecContext ctx)
    {
        return ctx.ValidTargets.Select(x => new TargetingContext
        {
            Target = x,
            TargetRank = 0
        });
    }
}