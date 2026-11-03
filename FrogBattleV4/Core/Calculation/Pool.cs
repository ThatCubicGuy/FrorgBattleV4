using System;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Calculation;

public sealed class Pool()
{
    private double _currentValue;

    /// <summary>
    /// Instantiates a new instance of a pool with the capacity of another and CurrentValue = 0.
    /// </summary>
    /// <param name="other">Pool to copy.</param>
    public Pool(Pool other) : this()
    {
        Capacity = other.Capacity;
    }

    public double Capacity { get; set; }

    [JsonIgnore]
    public double Value
    {
        get => _currentValue;
        set => _currentValue = Math.Clamp(value, 0, Capacity);
    }
}