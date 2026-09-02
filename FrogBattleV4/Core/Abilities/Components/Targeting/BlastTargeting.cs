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
    public int Radius { get; init; } = 1;

    /// <summary>
    /// How much the rank increases for each slot away from the main target.
    /// </summary>
    public int RankPenaltyPerSlot { get; init; } = 1;

    /// <summary>
    /// The maximum rank that targets can have to be eligible for adjacent selection.
    /// </summary>
    public int MaximumRank { get; init; } = 99;

    public TargetingSelection SelectTargets(LinkResolutionState state, BattleEnvironment env)
    {
        var result = new List<AbilityTargetingContext>();
        foreach (var (target, rank) in state.Selections.Where(atc => atc.Rank <= MaximumRank))
        {
            var allies = env.GetNeighbors(target);
            var idx = allies.IndexOf(target);
            result.AddRange(allies
                .Skip(Math.Max(0, idx - Radius))
                .Take(2 * Radius + 1)
                .Select((member, i) => new AbilityTargetingContext
                {
                    Target = member,
                    Rank = rank + Math.Abs(idx - i) * RankPenaltyPerSlot,
                }));
        }

        return new TargetingSelection(result);
    }
}