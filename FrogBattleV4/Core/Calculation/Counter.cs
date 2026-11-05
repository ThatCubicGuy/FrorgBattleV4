using System;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Calculation;

public sealed class Counter()
{
    private readonly long _defaultValue;
    private long _currentValue;

    public event EventHandler? TriggerValueReached;
    internal event EventHandler<ValueChangedEventArgs>? ValueChanged;

    /// <summary>
    /// Instantiates a new instance of a counter with the max value, default value,
    /// trigger value, and trigger condition of another, and CurrentValue = 0.
    /// </summary>
    /// <param name="other">Counter to copy.</param>
    public Counter(Counter other) : this()
    {
        DefaultValue = other.DefaultValue;
        TriggerValue = other.TriggerValue;
        TriggerCondition = other.TriggerCondition;
        MaxValue = other.MaxValue;
    }

    [JsonPropertyName("default")]
    public long DefaultValue
    {
        get => _defaultValue;
        init => CurrentValue = _defaultValue = value;
    }

    /// <summary>
    /// Value at which the TriggerValueReached event is invoked.
    /// </summary>
    [JsonPropertyName("trigger")]
    public long? TriggerValue { get; init; }

    /// <summary>
    /// Determines if the TriggerValueReached event should be triggered outside strict equality.
    /// </summary>
    [JsonPropertyName("condition")]
    public TriggerType TriggerCondition { get; init; } = TriggerType.Above;

    /// <summary>
    /// Maximum value that the counter stops increasing at.
    /// Null means no limit.
    /// </summary>
    [JsonPropertyName("max")]
    public long? MaxValue { get; init; }

    [JsonIgnore]
    public long CurrentValue
    {
        get => _currentValue;
        set
        {
            var oldValue = _currentValue;
            _currentValue = MaxValue.HasValue ? Math.Clamp(value, 0L, MaxValue.Value) : Math.Max(value, 0L);

            if (_currentValue == oldValue) return;

            OnValueChanged(new ValueChangedEventArgs
            {
                OldValue = oldValue,
                NewValue = _currentValue,
            });

            // Avoid invoking TriggerValueReached event continuously
            if (IsTrigger(_currentValue) && !IsTrigger(oldValue))
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

    private bool IsTrigger(long value)
    {
        return TriggerCondition switch
        {
            TriggerType.Equal => value == TriggerValue,
            TriggerType.Below => value <= TriggerValue,
            TriggerType.Above => value >= TriggerValue,
            _ => false,
        };
    }

    /// <summary>
    /// Specifies inclusive comparisons against trigger value.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<TriggerType>))]
    public enum TriggerType
    {
        [JsonStringEnumMemberName("equal")] Equal,
        [JsonStringEnumMemberName("below")] Below,
        [JsonStringEnumMemberName("above")] Above,
    }

    private void OnValueChanged(ValueChangedEventArgs e)
    {
        ValueChanged?.Invoke(this, e);
    }

    private void OnTriggerValueReached()
    {
        TriggerValueReached?.Invoke(this, EventArgs.Empty);
    }

    internal class ValueChangedEventArgs : EventArgs
    {
        public required long OldValue { get; init; }
        public required long NewValue { get; init; }
    }
}