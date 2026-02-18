#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.AbilitySystem;

namespace FrogBattleV4.Core.DamageSystem;

/// <summary>
/// Stores the context in which any damage will be taken.
/// </summary>
public readonly struct DamageExecContext
{
    public required IBattleMember? Source { get; init; }
    public required AbilityDefinition? Definition { get; init; }
    [NotNull] public required Random Rng { get; init; }
}