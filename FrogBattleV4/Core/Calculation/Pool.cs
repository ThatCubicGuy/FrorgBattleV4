using System;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Calculation;

public sealed class Pool(double capacity)
{
    private double _currentValue;

    public double Capacity { get; set; } = capacity;

    [JsonIgnore]
    public double Value
    {
        get => _currentValue;
        set => _currentValue = Math.Clamp(value, 0, Capacity);
    }
}