using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Calculation.Pools;

public struct PoolInitContext
{
    public required IPoolDefinition Definition { get; init; }
    public GameEntity Target { get; init; }
    // how to be an ABSOLUTE rat bastard.
#nullable enable
    public GameEntity? Source { get; init; }
}