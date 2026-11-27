namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Mutation query classifying a pool mutation (e.g. healing, spending mana).
/// </summary>
public class MutateQuery : CommandQuery
{
    public required PoolName Pool { get; init; }
    public required PoolMutChannel Channel { get; init; }
}