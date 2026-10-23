namespace FrogBattleV4.Core.Abilities.Targeting;

/// <summary>
/// Filters targets already selected based on
/// a given predicate and a maximum rank and
/// applies a rank penalty after filtering.
/// </summary>
public class FilterTargeting : IShardTargeting
{
    /// <summary>
    /// Targets every already selected target at the same rank.
    /// Basically, leaves everything unchanged.
    /// </summary>
    public static FilterTargeting Identity { get; } = new();

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
    public EntityPredicate Filter { get; init; }

    public TargetingInstruction GenerateInstruction(ShardTargetingContext targeting, BattleEnvironment env)
    {
        var (target, rank) = targeting;
        if (rank <= MaximumRank && env.IsPredicate(target, Filter))
            return TargetingInstruction.From(targeting with { Rank = rank + RankPenalty });

        return TargetingInstruction.Empty;
    }
}