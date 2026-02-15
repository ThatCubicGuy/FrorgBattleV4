using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.DamageSystem;

public readonly struct DamageIntent()
{
    public required double BaseAmount { get; init; }
    [NotNull] public required IDamageable Target { get; init; }
    [NotNull] public required DamageProperties Properties { get; init; }
    [NotNull] public required TargetingType Targeting { get; init; }
    public bool CanCrit { get; init; } = true;
}