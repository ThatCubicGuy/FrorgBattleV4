using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Targeting;

/// <summary>
/// Selects an entire team.
/// </summary>
public class AreaOfEffectTargeting : IShardTargeting
{
    public required int RankPenalty { get; init; }

    public IEnumerable<ShardTargetingContext> SelectTargets(ShardResolutionScope scope, BattleEnvironment env)
    {
        return env.GetNeighbors(scope.Targeting.Target)
            .Select(entity => new ShardTargetingContext
            {
                Target = entity,
                Rank = scope.Targeting.Rank + RankPenalty,
            });
    }

    public TargetingInstruction GenerateInstruction(ShardResolutionScope scope, BattleEnvironment env)
    {
        return new TargetingInstruction.Fixed(env.GetNeighbors(scope.Targeting.Target)
            .Select(entity => new ShardTargetingContext
            {
                Target = entity,
                Rank = scope.Targeting.Rank + RankPenalty,
            }));
    }
}