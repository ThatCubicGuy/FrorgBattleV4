using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components;

public struct AbilityTargetingContext
{
    /// <summary>
    /// The target of this hit.
    /// </summary>
    [NotNull] public required IBattleMember Target { get; init; }
    /// <summary>
    /// The targeting that we are going to use to attack this member.
    /// </summary>
    [NotNull] public required TargetingType Aiming { get; init; }
    /// <summary>
    /// The rank of the target signifies whether it
    /// is the primary target (rank 0), secondary, or further.
    /// Especially useful for blast attacks. Most attacks don't go past rank 1.
    /// </summary>
    public required int Rank { get; init; }
}