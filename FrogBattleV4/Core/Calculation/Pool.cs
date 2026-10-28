using System;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Calculation;

public class Pool
{
    private double _currentValue;

    public event EventHandler<PoolValueChangedEventArgs>? ValueChanged;

    public double Capacity { get; set; }

    [JsonIgnore]
    public double Value
    {
        get => _currentValue;
        set
        {
            var oldValue = _currentValue;
            _currentValue = Math.Clamp(value, 0, Capacity);
            OnValueChanged(new PoolValueChangedEventArgs
            {
                OldValue = oldValue,
                NewValue = _currentValue
            });
        }
    }

    protected virtual void OnValueChanged(PoolValueChangedEventArgs e)
    {
        ValueChanged?.Invoke(this, e);
    }
}

public class PoolValueChangedEventArgs : EventArgs
{
    public required double OldValue { get; init; }
    public required double NewValue { get; init; }
}