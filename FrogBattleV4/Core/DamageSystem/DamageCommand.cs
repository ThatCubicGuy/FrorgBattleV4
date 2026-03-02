using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;

namespace FrogBattleV4.Core.DamageSystem;

/// <summary>
/// Created by attack components and sent upstream to the system to calculate.
/// </summary>
public record DamageCommand : IBattleCommand
{
    public required double BaseAmount { get; init; }
    public required DamageType Type { get; init; }
    [NotNull] public required IBattleMember Target { get; init; }
    [NotNull] public required TargetingType Targeting { get; init; }
    public bool CanCrit { get; init; } = true;
}