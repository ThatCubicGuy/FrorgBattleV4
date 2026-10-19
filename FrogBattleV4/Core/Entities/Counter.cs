using System;

namespace FrogBattleV4.Core.Entities;

public class Counter
{
    private readonly long _defaultValue;
    private long _currentValue;

    public event EventHandler<CounterValueChangedArgs>? ValueChanged;
    public long DefaultValue
    {
        get => _defaultValue;
        init => CurrentValue = _defaultValue = value;
    }

    public long CurrentValue
    {
        get => _currentValue;
        set
        {
            var oldValue = _currentValue;
            _currentValue = MaxValue.HasValue ? Math.Clamp(value, 0L, MaxValue.Value) : Math.Max(value, 0L);
            ValueChanged?.Invoke(this, new CounterValueChangedArgs
            {
                OldValue = oldValue,
                NewValue = _currentValue
            });
        }
    }

    public long? MaxValue { get; init; }

    /// <summary>
    /// Tries incrementing the counter.
    /// </summary>
    /// <returns>True if the counter was incremented successfully, false otherwise.</returns>
    public bool TryIncrement()
    {
        if (CurrentValue >= MaxValue) return false;

        CurrentValue += 1;
        return true;
    }

    /// <summary>
    /// Tries decrementing the counter.
    /// </summary>
    /// <returns>True if the counter was decremented successfully, false otherwise.</returns>
    public bool TryDecrement()
    {
        if (CurrentValue <= 0) return false;

        CurrentValue -= 1;
        return true;
    }

    /// <summary>
    /// Tries consuming a certain amount from the counter. Does not consume anything on failure.
    /// </summary>
    /// <param name="amount">Amount to try consuming.</param>
    /// <returns>
    /// True if the current amount is greater or equal to <paramref name="amount"/>
    /// and was thus consumed, false otherwise.
    /// </returns>
    public bool TryConsume(long amount)
    {
        if (CurrentValue < amount) return false;

        CurrentValue -= amount;
        return true;
    }

    public void Reset()
    {
        CurrentValue = DefaultValue;
    }
}

public class CounterValueChangedArgs : EventArgs
{
    public required long OldValue { get; init; }
    public required long NewValue { get; init; }
}