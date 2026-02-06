using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.AbilitySystem;

/// <summary>
/// Enables the full execution of an ability.
/// TODO: Turn this ^ into this v [keep readonly]
/// Describes the context in which any ability may be executed.
/// </summary>
public readonly struct AbilityExecContext
{
    [NotNull] public required Character User { get; init; }
    [NotNull] public required IDamageable MainTarget { get; init; }

    /// <summary>
    /// Pool of targets that the ability's targeting components can select from, knowing the main target.
    /// </summary>
    /// <remarks>Order sensitive!</remarks>
    [NotNull]
    public required IEnumerable<IDamageable> ValidTargets { get; init; }

    [NotNull] public required AbilityDefinition Definition { get; init; }
    [NotNull] public required System.Random Rng { get; init; }
}