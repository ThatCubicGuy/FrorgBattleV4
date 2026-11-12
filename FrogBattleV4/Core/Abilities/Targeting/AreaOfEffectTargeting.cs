using System.Linq;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities.Targeting;

/// <summary>
/// Selects an entire team.
/// </summary>
public class AreaOfEffectTargeting : IShardTargeting
{
    [JsonPropertyName("penalty")]
    public required int RankPenalty { get; init; }

    public TargetingInstruction GenerateInstruction(ShardTargetingContext targeting, BattleEnvironment env)
    {
        var (target, rank) = targeting;
        return new TargetingInstruction.Fixed(env.GetNeighbors(target)
            .Select(entity => new ShardTargetingContext
            {
                Target = entity,
                Rank = rank + RankPenalty,
            }));
    }
}