using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.DamageSystem;

/// <summary>
/// Created by attack components and sent upstream to the system to calculate.
/// </summary>
public readonly struct DamageIntent()
{
    public required double BaseAmount { get; init; }
    public required DamageType Type { get; init; }
    [NotNull] public required IDamageable Target { get; init; }
    [NotNull] public required TargetingType Targeting { get; init; }
    public bool CanCrit { get; init; } = true;
}