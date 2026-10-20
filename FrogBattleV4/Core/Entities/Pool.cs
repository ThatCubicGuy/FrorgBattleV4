using System;

namespace FrogBattleV4.Core.Entities;

public class Pool
{
    private double _currentValue;

    public event EventHandler<ValueChangedEventArgs>? ValueChanged;
    public double Capacity { get; set; }

    public double Value
    {
        get => _currentValue;
        set
        {
            var oldValue = _currentValue;
            _currentValue = Math.Clamp(value, 0, Capacity);
            ValueChanged?.Invoke(this, new ValueChangedEventArgs
            {
                OldValue = oldValue,
                NewValue = _currentValue
            });
        }
    }

    public class ValueChangedEventArgs : EventArgs
    {
        public required double OldValue { get; init; }
        public required double NewValue { get; init; }
    }
}