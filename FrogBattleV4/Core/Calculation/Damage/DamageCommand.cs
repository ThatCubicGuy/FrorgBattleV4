using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Calculation.Damage;

/// <summary>
/// Represents a command for dealing damage to a target.
/// </summary>
public record DamageCommand
{
    public required double BaseAmount { get; init; }
    public required DamageType Type { get; init; }
    public required DamageSource Source { get; init; }
    public IBattleMember Attacker { get; init; }
    [NotNull] public required IBattleMember Target { get; init; }
    [NotNull] public required TargetingType Targeting { get; init; }
    public bool CanCrit { get; init; } = true;
}