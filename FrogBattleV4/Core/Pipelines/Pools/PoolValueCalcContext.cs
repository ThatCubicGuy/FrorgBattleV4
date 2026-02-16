#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.Pipelines.Pools;

/// <summary>
/// The context for calculating a pool-related stat.
/// This can be the pool's max value, a regen request, or a cost request.
/// </summary>
public readonly struct PoolValueCalcContext
{
    [NotNull] public required IHasPools Holder { get; init; }
    public BattleMember? Other { get; init; }
    public PoolMutationFlags Flags { get; init; }
}

[Flags] public enum PoolMutationFlags
{
    None = 0,
    Immutable = 1 << 0
}