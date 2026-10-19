using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Targeting;

public class BounceTargeting : IShardTargeting
{
    public required int Count { get; init; }
    public int RankPenalty { get; init; } = 0;

    public TargetingInstruction GenerateInstruction(ShardResolutionScope scope, BattleEnvironment env)
    {
        var (target, rank) = scope.Targeting;
        return TargetingInstruction.From(scope.Targeting)
            .WithRandom(env.GetNeighbors(target).Select(entity => new ShardTargetingContext
                {
                    Target = entity,
                    Rank = rank + RankPenalty
                }), Count);
    }
}