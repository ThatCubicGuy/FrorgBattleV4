using System.Collections.Generic;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class Single : ITargetingComponent
{
    public IEnumerable<TargetingContext> SelectTargets(AbilityContext ctx)
    {
        return
        [
            new TargetingContext
            {
                Target = ctx.MainTarget,
                TargetRank = 0
            }
        ];
    }
}