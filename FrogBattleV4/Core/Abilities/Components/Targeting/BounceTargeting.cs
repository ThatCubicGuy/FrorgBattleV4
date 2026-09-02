using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Targeting;

public class BounceTargeting : IShardTargeting
{
    public required int Count { get; init; }
    public int RankPenaltyPerBounce { get; init; } = 0;
    public TargetingSelection SelectTargets(LinkResolutionState state, BattleEnvironment env)
    {
        var targets = new List<AbilityTargetingContext>();
        foreach (var (target, rank) in state.Selections)
        {
            targets.Add(new AbilityTargetingContext
            {
                Target = target,
                Rank = rank,
            });
            for (var i = 0; i < Count; i++)
            {
                var next = env.GetNeighbors(target).MinBy(_ => env.NextRoll());
                targets.Add(new AbilityTargetingContext
                {
                    Target = next,
                    Rank = rank + i * RankPenaltyPerBounce,
                });
            }
        }

        return new TargetingSelection(targets);
    }
}