using System;
using System.Linq;
using System.Collections.Generic;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class Blast : ITargetingComponent
{
    public IEnumerable<ITargetable> SelectTargets(AbilityContext ctx, ITargetable mainTarget)
    {
        var index = ctx.ValidTargets.IndexOf(mainTarget);
        if (index == -1) throw new ArgumentException("Main target is not a valid target!", nameof(mainTarget));
        List<ITargetable> result = [mainTarget];
        if (index < ctx.ValidTargets.Count) result.Add(ctx.ValidTargets[index + 1]);
        return index > 0 ? result.Prepend(ctx.ValidTargets[index - 1]) : result;
    }
}