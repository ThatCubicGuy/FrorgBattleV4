using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Value query classifying a pool property (e.g. max value).
/// </summary>
public sealed record PoolStatQuery : StaticQuery
{
    public required PoolId PoolId { get; init; }
    public required PoolValueChannel Channel { get; init; }
}

public enum PoolValueChannel
{
    Max,
    Min
}