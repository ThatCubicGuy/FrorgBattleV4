namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Mutation query classifying a pool mutation (e.g. healing, spending mana).
/// </summary>
public readonly struct PoolMutQuery
{
    public required PoolId PoolId { get; init; }
    public required PoolMutChannel Channel { get; init; }
}

public enum PoolMutChannel
{
    Cost,
    Regen
}