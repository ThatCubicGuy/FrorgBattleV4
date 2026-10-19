using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Targeting;

/// <summary>
/// Selects targets adjacent to other selected ones
/// below a maximum rank.
/// </summary>
public class BlastTargeting : IShardTargeting
{
    // Select targets up to Radius slots away from the main target.
    public int Radius { get; init; } = 1;

    /// <summary>
    /// How much the rank increases for each slot away from the main target.
    /// </summary>
    public int RankPenaltyPerSlot { get; init; } = 1;

    /// <summary>
    /// The maximum rank that targets can have to be eligible for selection.
    /// </summary>
    public int MaximumRank { get; init; } = 99;

    public IEnumerable<ShardTargetingContext> SelectTargets(ShardResolutionScope scope, BattleEnvironment env)
    {
        var (target, rank) = scope.Targeting;
        if (rank > MaximumRank) return [];

        var allies = env.GetNeighbors(target).ToList();
        var idx = allies.IndexOf(target);

        return allies
            .Skip(Math.Max(0, idx - Radius))
            .Take(2 * Radius + 1)
            .Select((member, i) => new ShardTargetingContext
            {
                Target = member,
                Rank = rank + Math.Abs(idx - i) * RankPenaltyPerSlot,
            });
    }

    public TargetingInstruction GenerateInstruction(ShardResolutionScope scope, BattleEnvironment env)
    {
        var (target, rank) = scope.Targeting;
        if (rank > MaximumRank) return TargetingInstruction.Empty;

        var allies = env.GetNeighbors(target).ToList();
        var idx = allies.IndexOf(target);

        return new TargetingInstruction.Fixed(allies
            .Skip(Math.Max(0, idx - Radius))
            .Take(2 * Radius + 1)
            .Select((member, i) => new ShardTargetingContext
            {
                Target = member,
                Rank = rank + Math.Abs(idx - i) * RankPenaltyPerSlot,
            })
            .Where(targeting => targeting.Rank <= MaximumRank));
    }
}