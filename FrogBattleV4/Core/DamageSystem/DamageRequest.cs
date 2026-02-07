using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.DamageSystem;

public readonly struct DamageRequest()
{
    public required double BaseAmount { get; init; }
    [NotNull] public required IDamageable Target { get; init; }
    [NotNull] public required DamageProperties Properties { get; init; }
    public bool CanCrit { get; init; } = true;
}