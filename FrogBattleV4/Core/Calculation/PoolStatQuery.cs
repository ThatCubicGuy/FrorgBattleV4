using FrogBattleV4.Core.Effects.Modifiers;

namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Value query classifying a pool property (e.g. max value).
/// </summary>
public record PoolValueQuery : Query
{
    public required PoolId PoolId { get; init; }
    public required PoolValueChannel Channel { get; init; }
}

public enum PoolValueChannel
{
    Max,
    Min
}