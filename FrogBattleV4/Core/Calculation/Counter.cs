using System;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Calculation;

public class Counter
{
    private readonly long _defaultValue;
    private long _currentValue;

    public event EventHandler<ValueChangedEventArgs>? ValueChanged;
    public event EventHandler? TriggerValueReached;

    [JsonPropertyName("default")]
    public long DefaultValue
    {
        get => _defaultValue;
        init => CurrentValue = _defaultValue = value;
    }

    /// <summary>
    /// Value at which the TriggerValueReached event is invoked.
    /// </summary>
    public long? TriggerValue { get; init; }
    
    /// <summary>
    /// Determines if the TriggerValueReached event should be triggered outside strict equality.
    /// </summary>
    public TriggerType TriggerCondition { get; init; } = TriggerType.Equal;

    public long? MaxValue { get; init; }

    [JsonIgnore]
    public long CurrentValue
    {
        get => _currentValue;
        set
        {
            var oldValue = _currentValue;
            _currentValue = MaxValue.HasValue ? Math.Clamp(value, 0L, MaxValue.Value) : Math.Max(value, 0L);
            OnValueChanged(new ValueChangedEventArgs
            {
                OldValue = oldValue,
                NewValue = _currentValue
            });
            if (TriggerCondition switch
                {
                    TriggerType.Equal => _currentValue == TriggerValue,
                    TriggerType.LessThan => _currentValue <= TriggerValue,
                    TriggerType.GreaterThan => _currentValue >= TriggerValue,
                    _ => false
                })
            {
                OnTriggerValueReached();
            }
        }
    }

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

    /// <summary>
    /// Resets the counter back to its default value.
    /// </summary>
    public void Reset()
    {
        CurrentValue = DefaultValue;
    }

    /// <summary>
    /// Specifies inclusive comparisons against trigger value.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<TriggerType>))]
    public enum TriggerType
    {
        [JsonStringEnumMemberName("equal")]
        Equal,
        [JsonStringEnumMemberName("under")]
        LessThan,
        [JsonStringEnumMemberName("over")]
        GreaterThan,
    }

    public class ValueChangedEventArgs : EventArgs
    {
        public required long OldValue { get; init; }
        public required long NewValue { get; init; }
    }

    protected virtual void OnValueChanged(ValueChangedEventArgs e)
    {
        ValueChanged?.Invoke(this, e);
    }

    protected virtual void OnTriggerValueReached()
    {
        TriggerValueReached?.Invoke(this, EventArgs.Empty);
    }
}