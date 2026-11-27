using System.Linq;

namespace FrogBattleV4.Core.Abilities.Targeting;

public class BounceTargeting : IAbilityTargeting
{
    public required int Count { get; init; }
    public int RankPenalty { get; init; }

    public TargetingInstruction GenerateInstruction(TargetingContext targeting, BattleEnvironment env)
    {
        var (target, rank) = targeting;
        return TargetingInstruction.From(targeting)
            .WithRandom(env.GetNeighbors(target).Select(entity => new TargetingContext
                {
                    Target = entity,
                    Rank = rank + RankPenalty,
                }), Count);
    }
}