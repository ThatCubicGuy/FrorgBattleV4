using System.Linq;
using System.Collections.Generic;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class Blast : ITargetingComponent
{
    public IEnumerable<ITargetable> SelectTargets(AbilityContext ctx)
    {
        var index = ctx.ValidTargets.IndexOf(ctx.MainTarget);
        if (index == -1) throw new System.ArgumentException("Main target is not a valid target!", nameof(ctx.MainTarget));
        List<ITargetable> result = [ctx.MainTarget];
        if (index < ctx.ValidTargets.Count) result.Add(ctx.ValidTargets[index + 1]);
        return index > 0 ? result.Prepend(ctx.ValidTargets[index - 1]) : result;
    }
}