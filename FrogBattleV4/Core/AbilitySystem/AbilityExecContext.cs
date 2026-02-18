using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem;

/// <summary>
/// Enables the full execution of an ability.
/// TODO: Turn this ^ into this v [keep readonly]
/// Describes the context in which any ability may be executed.
/// </summary>
public readonly struct AbilityExecContext
{
    [NotNull] public required IBattleMember User { get; init; }
    [NotNull] public required IBattleMember MainTarget { get; init; }

    /// <summary>
    /// Pool of targets that the ability's targeting components can select from, knowing the main target.
    /// </summary>
    /// <remarks>Order sensitive!</remarks>
    [NotNull] public required IEnumerable<IBattleMember> ValidTargets { get; init; }

    [NotNull] public required AbilityDefinition Definition { get; init; }
    [NotNull] public required Random Rng { get; init; }
}