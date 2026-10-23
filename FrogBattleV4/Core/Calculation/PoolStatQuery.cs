namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Value query classifying a pool property (e.g. max value).
/// </summary>
public sealed class PoolStatQuery : AttributeQuery
{
    public required PoolName Pool { get; init; }
    public required PoolStatChannel Channel { get; init; }
}

public enum PoolStatChannel
{
    Max
}