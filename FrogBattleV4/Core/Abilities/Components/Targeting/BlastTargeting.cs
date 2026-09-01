using System;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Targeting;

/// <summary>
/// Selects targets adjacent to other selected ones
/// below a maximum rank.
/// </summary>
public class BlastTargeting : IShardTargeting
{
    // Select targets up to Radius slots away from the main target.
    public int Radius { get; set; } = 1;

    /// <summary>
    /// How much the rank increases for each slot away from the main target.
    /// </summary>
    public int RankPenaltyPerSlot { get; set; } = 1;

    /// <summary>
    /// The maximum rank that targets can have to be eligible for adjacent selection.
    /// </summary>
    public int MaximumRank { get; set; } = 99;

    public IEnumerable<AbilityTargetingContext> SelectTargets(ShardLinkContext ctx)
    {
        var result = new List<AbilityTargetingContext>();
        foreach (var (target, rank) in ctx.Targets.Where(atc => atc.Rank <= MaximumRank))
        {
            var team = ctx.State.AlliedTeamOf(target).Members;
            var idx = team.IndexOf(target);
            result.AddRange(team
                .Skip(Math.Max(0, idx - Radius))
                .Take(2 * Radius + 1)
                .Select((entity, i) => new AbilityTargetingContext
                {
                    Target = entity,
                    Rank = rank + Math.Abs(idx - i) * RankPenaltyPerSlot,
                }));
        }

        return result;
    }
}