using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FrogBattleV4.Core.Calculation.Pools;

public class PoolComponent(PoolInitContext ctx)
{
    private double _currentValue = ctx.Definition.GetInitialValue(new InteractionContext(ctx.Target));

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
            if (_currentValue <= 0)
            {
                MinReached?.Invoke(this, -1 * _currentValue);
                _currentValue = Math.Max(_currentValue, 0);
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