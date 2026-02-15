using System.Collections.Generic;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class SingleTargeting(TargetingType type) : ITargetingComponent
{
    public IEnumerable<AbilityTargetingContext> SelectTargets(AbilityExecContext ctx)
    {
        yield return new AbilityTargetingContext
        {
            Target = ctx.MainTarget,
            Aiming = type,
            Rank = 0,
        };
    }
}