using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Targeting;

/// <summary>
/// Selects an entire team.
/// </summary>
public class AreaOfEffectTargeting : IShardTargeting
{
    public required int RankPenalty { get; set; }

    public IEnumerable<AbilityTargetingContext> SelectTargets(ShardLinkContext ctx)
    {
        var result = new List<AbilityTargetingContext>();
        foreach (var (target, rank) in ctx.Targets)
        {
            result.AddRange(ctx.State
                .AlliedTeamOf(target).Members
                .Select(entity => new AbilityTargetingContext
                {
                    Target = entity,
                    Rank = rank + RankPenalty,
                }));
        }

        return result;
    }
}