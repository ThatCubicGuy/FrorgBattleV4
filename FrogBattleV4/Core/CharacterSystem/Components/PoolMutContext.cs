namespace FrogBattleV4.Core.CharacterSystem.Components;

public struct PoolMutContext
{
    public required string PoolId { get; init; }
    public required PoolModType Channel { get; init; }
}

public enum PoolModType
{
    Max,
    Cost,
    Regen
}