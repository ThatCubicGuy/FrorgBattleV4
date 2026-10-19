using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Targeting;

public class BounceTargeting : IShardTargeting
{
    public required int Count { get; init; }
    public int RankPenalty { get; init; } = 0;
    public IEnumerable<ShardTargetingContext> SelectTargets(ShardResolutionScope scope, BattleEnvironment env)
    {
        var (target, rank) = scope.Targeting;
        yield return new ShardTargetingContext
        {
            Target = target,
            Rank = rank,
        };

        var neighbors = env.GetNeighbors(target).ToArray();
        for (var i = 0; i < Count; i++)
        {
            yield return new ShardTargetingContext
            {
                Target = scope.Rng.Choose(neighbors),
                Rank = rank + i * RankPenalty,
            };
        }
    }

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