#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FrogBattleV4.Core.Calculation.Pools;

public class PoolComponent(PoolInitContext ctx)
{
    private double _currentValue = ctx.Definition.GetInitialValue(new ModifierContext(ctx.Target));
    // Context for calculating capacity is always identical

    public event Action<PoolComponent, double, double>? ValueChanged;
    public event Action<PoolComponent, double>? MinReached;
    public event Action<PoolComponent, double>? MaxReached;

    public HashSet<PoolTag> Tags { get; } = [..ctx.Definition.Tags];

    public double CurrentValue
    {
        get => _currentValue;
        set
        {
            var old = _currentValue;
            _currentValue = value;
            if (_currentValue <= MinValue)
            {
                MinReached?.Invoke(this, MinValue - _currentValue);
                _currentValue = Math.Max(_currentValue, MinValue);
            }
            if (_currentValue >= MaxValue)
            {
                MaxReached?.Invoke(this, _currentValue - MaxValue);
                _currentValue = Math.Min(_currentValue, MaxValue);
            }
            if (!old.Equals(_currentValue)) ValueChanged?.Invoke(this, old, _currentValue);
        }
    }

    public double MaxValue { get; } = ctx.Definition.MaxValue;
    public double MinValue { get; } = ctx.Definition.MinValue;

    // I like having both '?' and [NotNull] tags in my code, so in
    // this context where I only have one nullable reference field
    // I won't enable file-wide nullable annotations because Rider
    // will give me warnings about redundant [NotNull] attributes.
#nullable disable
    public bool HasTag(PoolTag tag)
    {
        return Tags.Contains(tag);
    }

    public bool HasAllTags([NotNull] params PoolTag[] tags)
    {
        return Tags.IsSupersetOf(tags);
    }

    public bool HasAnyTags([NotNull] params PoolTag[] tags)
    {
        return tags.Any(tag => Tags.Contains(tag));
    }
}