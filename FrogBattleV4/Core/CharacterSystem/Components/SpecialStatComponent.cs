namespace FrogBattleV4.Core.CharacterSystem.Components;

public class SpecialStatComponent : IPoolComponent
{
    public required string Id { get; init; }
    public double CurrentValue { get; set; }
    public double? MaxValue { get; init; }
    public PoolFlags Flags { get; init; } = PoolFlags.Dummy;
}