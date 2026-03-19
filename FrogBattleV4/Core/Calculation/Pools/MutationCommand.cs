using System;
using FrogBattleV4.Core.Abilities;

namespace FrogBattleV4.Core.Calculation.Pools;

/// <summary>
/// Initializes a new mutation command with the given properties.
/// </summary>
/// <param name="TargetPool">The ID of the pool to mutate.</param>
/// <param name="BaseAmount">Base amount of the mutation.</param>
/// <param name="Flags">Mutation flags, such as... immutability of the mutation.</param>
public record MutationCommand(
    IBattleMember Target,
    double BaseAmount,
    PoolId TargetPool,
    PoolMutationFlags Flags = PoolMutationFlags.None) : IBattleCommand;

[Flags] public enum PoolMutationFlags
{
    None = 0,
    Immutable = 1 << 0
}