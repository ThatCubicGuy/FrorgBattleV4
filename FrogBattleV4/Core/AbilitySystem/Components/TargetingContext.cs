using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components;

public struct TargetingContext
{
    public required ITargetable Target { get; init; }
    /// <summary>
    /// The rank of the target signifies whether it
    /// is the primary target (rank 0), secondary, or further.
    /// Especially useful for blast attacks. Most attacks don't go past rank 1.
    /// </summary>
    public required int TargetRank { get; init; }
}