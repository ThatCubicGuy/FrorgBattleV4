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
    public required IBattleMember? Source { get; init; }
    public required IBattleMember? Other { get; init; }
    [NotNull] public required Random Rng { get; init; }
    IBattleMember? IActorContext.Actor => Source;
    IBattleMember? IReferenceContext.Other => Other;
}