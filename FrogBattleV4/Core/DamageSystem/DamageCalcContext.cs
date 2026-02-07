#nullable enable
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.DamageSystem;

/// <summary>
/// A context for calculating damage modifiers.
/// Randomness is already computed.
/// </summary>
public readonly struct DamageCalcContext() : IRelationshipContext
{
    public IBattleMember? Attacker { get; init; }
    public IBattleMember? Other { get; init; }
    // public DamageProperties Properties { get; init; }
    // It might make sense to forgo DamageProperties, honestly.
    [NotNull] public string Type { get; init; }
    [NotNull] public string Source { get; init; }
    public required bool IsCrit { get; init; }
    public double DefPen { get; init; } = 0;
    public double TypeResPen { get; init; } = 0;
    IBattleMember? IActorContext.Actor => Attacker;
    IBattleMember? IReferenceContext.Other => Other;
}