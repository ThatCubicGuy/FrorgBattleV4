using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.Pipelines;

[method: SetsRequiredMembers]
public readonly struct PoolValueQuery(PoolId poolId, PoolPropertyChannel channel)
{
    public required PoolId PoolId { get; init; } = poolId;
    public required PoolPropertyChannel Channel { get; init; } = channel;
}

public enum PoolPropertyChannel
{
    Max,
    Cost,
    Regen
}