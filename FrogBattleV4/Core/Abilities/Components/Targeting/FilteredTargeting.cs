using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Targeting;

/// <summary>
/// Filters targets already selected based on
/// a maximum rank and applies a rank penalty
/// on top of them after filtering.
/// </summary>
public class FilteredTargeting : IShardTargeting
{
    /// <summary>
    /// Additional rank added to each selected target after filtering.
    /// </summary>
    public int RankPenalty { get; set; } = 0;

    /// <summary>
    /// The maximum rank that targets can have to be eligible for selection.
    /// </summary>
    public int MaximumRank { get; set; } = 99;

    public IEnumerable<AbilityTargetingContext> SelectTargets(ShardLinkContext ctx)
    {
        return ctx.Targets
            .Where(atc => atc.Rank <= MaximumRank)
            .Select(atc => atc with { Rank = atc.Rank + RankPenalty });
    }
}