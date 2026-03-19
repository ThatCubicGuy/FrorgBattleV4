using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Combat;

namespace FrogBattleV4.Core.Calculation.Damage;

/// <summary>
/// Created by attack components and sent upstream to the system to calculate.
/// </summary>
public record DamageCommand : IBattleCommand
{
    public required double BaseAmount { get; init; }
    public required DamageType Type { get; init; }
    public required DamageSource Source { get; init; }
    [NotNull] public required IBattleMember Target { get; init; }
    [NotNull] public required TargetingType Targeting { get; init; }
    public bool CanCrit { get; init; } = true;
}