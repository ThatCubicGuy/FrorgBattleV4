#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.CharacterSystem.Pools;

/// <summary>
/// The context for calculating a pool-related stat.
/// This can be the pool's max value, a regen request, or a cost request.
/// </summary>
public readonly struct PoolValueCalcContext : IRelationshipContext
{
    [NotNull] public required string PoolId { get; init; }
    [NotNull] public required BattleMember Actor { get; init; }
    public BattleMember? Other { get; init; }
    public PoolPropertyChannel Channel { get; init; }
    public PoolMutationFlags Flags { get; init; }
}

public enum PoolPropertyChannel
{
    Max,
    Cost,
    Regen
}

[Flags] public enum PoolMutationFlags
{
    None = 0,
    Immutable = 1 << 0
}