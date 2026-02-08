#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.DamageSystem;

/// <summary>
/// Stores the context in which any damage will be taken.
/// </summary>
public readonly struct DamageExecContext : IRelationshipContext
{
    public required BattleMember? Source { get; init; }
    public required BattleMember? Other { get; init; }
    [NotNull] public required Random Rng { get; init; }
    BattleMember? IActorContext.Actor => Source;
    BattleMember? IReferenceContext.Other => Other;
}