using System.Linq;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Abilities.Targeting;

/// <summary>
/// Selects an entire team.
/// </summary>
public class AreaOfEffectTargeting : IAbilityTargeting
{
    [JsonPropertyName("penalty")]
    public required int RankPenalty { get; init; }

    public TargetingInstruction GenerateInstruction(TargetingContext targeting, BattleEnvironment env)
    {
        var (target, rank) = targeting;
        return new TargetingInstruction.Fixed(env.GetNeighbors(target)
            .Select(entity => new TargetingContext
            {
                Target = entity,
                Rank = rank + RankPenalty,
            }));
    }
}