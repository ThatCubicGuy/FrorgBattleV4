using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Targeting;

/// <summary>
/// Filters targets already selected based on
/// a given predicate and a maximum rank and
/// applies a rank penalty after filtering.
/// </summary>
public class FilteredTargeting : IShardTargeting
{
    /// <summary>
    /// Targets every already selected target at the same rank.
    /// Basically, leaves everything unchanged.
    /// </summary>
    public static FilteredTargeting Identity { get; } = new();

    /// <summary>
    /// Additional rank added to each selected target after filtering.
    /// </summary>
    public int RankPenalty { get; init; } = 0;

    /// <summary>
    /// The maximum rank that targets can have to be eligible for selection.
    /// </summary>
    public int MaximumRank { get; init; } = 99;

    /// <summary>
    /// Extra filter to apply to each target.
    /// </summary>
    public System.Func<AbilityTargetingContext, BattleEnvironment, bool> TargetFilter { get; init; } = (_, _) => true;

    public TargetingSelection SelectTargets(LinkResolutionState state, BattleEnvironment env)
    {
        return new TargetingSelection(state.Selections
            .Where(atc => atc.Rank <= MaximumRank && TargetFilter(atc, env))
            .Select(atc => atc with { Rank = atc.Rank + RankPenalty }));
    }
}