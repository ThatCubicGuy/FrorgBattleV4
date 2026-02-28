using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.Calculation.Pools;

public struct PoolInitContext
{
    [NotNull] public required IPoolDefinition Definition { get; init; }
    [NotNull] public IBattleMember Target { get; init; }
    // how to be an ABSOLUTE rat bastard.
#nullable enable
    public IBattleMember? Source { get; init; }
}