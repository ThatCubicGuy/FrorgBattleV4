using FrogBattleV4.Core.Effects.Modifiers;

namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Mutation query classifying a pool mutation (e.g. healing, spending mana).
/// </summary>
public record PoolMutQuery : MutationQuery
{
    /// <summary>
    /// ID of the pool.
    /// </summary>
    public required PoolId PoolId { get; init; }
    /// <summary>
    /// Channel on which this mutation will be registered (Cost/Regen)
    /// </summary>
    public required PoolMutChannel Channel { get; init; }
}

public enum PoolMutChannel
{
    Cost,
    Regen
}