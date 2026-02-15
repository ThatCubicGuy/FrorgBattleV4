using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.AbilitySystem.Components;

public struct AbilityTargetingContext
{
    /// <summary>
    /// The target of this hit.
    /// </summary>
    [NotNull] public required BattleMember Target { get; init; }
    /// <summary>
    /// The rank of the target signifies whether it
    /// is the primary target (rank 0), secondary, or further.
    /// Especially useful for blast attacks. Most attacks don't go past rank 1.
    /// </summary>
    public required int TargetRank { get; init; }
    /// <summary>
    /// Modifiers applied to the enemy due to being attacked through this targetable.
    /// </summary>
    [NotNull] public IEnumerable<IModifierRule> Modifiers { get; init; }
}