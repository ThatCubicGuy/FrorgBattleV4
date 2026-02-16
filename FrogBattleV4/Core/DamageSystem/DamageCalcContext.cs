#nullable enable
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.DamageSystem;

/// <summary>
/// A context for calculating damage modifiers.
/// Randomness is already computed.
/// </summary>
public readonly struct DamageCalcContext()
{
    public BattleMember? Attacker { get; init; }
    public BattleMember? Other { get; init; }
    // public DamageProperties Properties { get; init; }
    // It might make sense to forgo DamageProperties, honestly.
    [NotNull] public string Type { get; init; }
    [NotNull] public string Source { get; init; }
    public required bool IsCrit { get; init; }
    public double DefPen { get; init; } = 0;
    public double TypeResPen { get; init; } = 0;
}