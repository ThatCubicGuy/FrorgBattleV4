using System.Collections.Generic;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class AreaOfEffect : ITargetingComponent
{
    public IEnumerable<ITargetable> SelectTargets(AbilityContext ctx, ITargetable mainTarget)
    {
        return ctx.ValidTargets;
    }
}