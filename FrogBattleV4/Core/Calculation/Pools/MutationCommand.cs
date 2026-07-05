using System;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Calculation.Pools;

/// <summary>
/// Initializes a new mutation command with the given properties.
/// </summary>
public record MutationCommand
{
    public IBattleMember Target { get; init; }
    /// <summary>The ID of the pool to mutate.</summary>
    public PoolId TargetPool { get; init; }
    /// <summary>Base amount of the mutation.</summary>
    public double BaseAmount { get; init; }
    /// <summary>Mutation flags, such as... immutability of the mutation.</summary>
    public PoolMutationFlags Flags { get; init; }
}

[Flags] public enum PoolMutationFlags
{
    None = 0,
    Immutable = 1 << 0
}