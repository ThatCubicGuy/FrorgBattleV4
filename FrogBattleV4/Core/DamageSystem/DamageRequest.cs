using System;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.DamageSystem;

public readonly struct DamageRequest() : IRngContext
{
    public required double BaseAmount { get; init; }
    [NotNull] public required IDamageable Target { get; init; }
    [NotNull] public required Random Rng { get; init; }
    [NotNull] public required DamageProperties Properties { get; init; }
    public bool CanCrit { get; init; } = true;
}