using System;

namespace FrogBattleV4.Core.CharacterSystem.Components;

public class SpecialStatComponent : IPoolComponent
{
    private double _currentValue;
    public required string Id { get; init; }

    public double CurrentValue
    {
        get => _currentValue;
        set => _currentValue = Math.Clamp(value, MinValue ?? double.MinValue, MaxValue ?? double.MaxValue);
    }
    public double? MaxValue { get; init; }
    public double? MinValue { get; init; }
    public PoolFlags Flags { get; init; } = PoolFlags.Dummy;
    public bool ModifyValue(double value, SpendContext ctx)
    {
        
    }
}