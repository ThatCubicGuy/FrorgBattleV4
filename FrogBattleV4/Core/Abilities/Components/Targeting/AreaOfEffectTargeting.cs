using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Targeting;

/// <summary>
/// Selects an entire team.
/// </summary>
public class AreaOfEffectTargeting : IShardTargeting
{
    public required int RankPenalty { get; init; }

    public TargetingSelection SelectTargets(LinkResolutionState state, BattleEnvironment env)
    {
        var result = new List<AbilityTargetingContext>();
        foreach (var (target, rank) in state.Selections)
        {
            result.AddRange(env.GetNeighbors(target)
                .Select(entity => new AbilityTargetingContext
                {
                    Target = entity,
                    Rank = rank + RankPenalty,
                }));
        }

        return new TargetingSelection(result);
    }
}