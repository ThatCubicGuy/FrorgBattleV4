using System.Collections.Generic;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components.Targeting;

public class Bounce : ITargetingComponent
{
    public required uint Count { get; init; }

    public IEnumerable<ITargetable> SelectTargets(AbilityContext ctx, ITargetable mainTarget)
    {
        List<ITargetable> result = [mainTarget];
        for (var i = 1; i < Count; i++)
        {
            result.Add(ctx.ValidTargets[ctx.Rng.Next(ctx.ValidTargets.Count)]);
        }
        return result;
    }
}