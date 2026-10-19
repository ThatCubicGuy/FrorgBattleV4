using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Targeting;

/// <summary>
/// Selects an entire team.
/// </summary>
public class AreaOfEffectTargeting : IShardTargeting
{
    public required int RankPenalty { get; init; }

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