using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class AreaOfEffect : ITargetingComponent
{
    public IEnumerable<TargetingContext> SelectTargets(AbilityContext ctx)
    {
        return ctx.ValidTargets.Select(x => new TargetingContext()
        {
            Target = x,
            TargetRank = 0
        });
    }
}