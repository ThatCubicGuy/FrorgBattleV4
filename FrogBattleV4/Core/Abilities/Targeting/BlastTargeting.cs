using System;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Targeting;

/// <summary>
/// Selects targets adjacent to other selected ones
/// below a maximum rank.
/// </summary>
public class BlastTargeting : IShardTargeting
{
    /// <summary>
    /// Maximum distance from the main target to select new targets.
    /// </summary>
    public int Radius { get; init; } = 1;

    /// <summary>
    /// How much the rank increases for each slot away from the main target.
    /// </summary>
    public int RankPenaltyPerSlot { get; init; } = 1;

    public TargetingInstruction GenerateInstruction(ShardTargetingContext targeting, BattleEnvironment env)
    {
        var (target, rank) = targeting;

        var allies = env.GetNeighbors(target).ToList();
        var idx = allies.IndexOf(target);

        return new TargetingInstruction.Fixed(allies
            .Skip(Math.Max(0, idx - Radius))
            .Take(2 * Radius + 1)
            .Select((member, i) => new ShardTargetingContext
            {
                Target = member,
                Rank = rank + Math.Abs(idx - i) * RankPenaltyPerSlot,
            }));
    }
}