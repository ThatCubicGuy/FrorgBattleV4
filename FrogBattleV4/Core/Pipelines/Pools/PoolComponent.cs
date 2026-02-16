#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.EffectSystem.StatusEffects;

namespace FrogBattleV4.Core.Pipelines.Pools;

public class PoolComponent
{
    private double _currentValue;
    private readonly HashSet<PoolTag> _tags = [];
    public event Action<PoolComponent, double, double>? ValueChanged;
    public required PoolId Id { get; init; }

    public double CurrentValue
    {
        get => _currentValue;
        set
        {
            var old = _currentValue;
            _currentValue = Math.Clamp(value, MinValue ?? double.MinValue, MaxValue ?? double.MaxValue);
            if (!old.Equals(_currentValue)) ValueChanged?.Invoke(this, _currentValue, value);
        }
    }

    public virtual double? MaxValue { get; init; }
    public virtual double? MinValue { get; init; }

    [NotNull] public IEnumerable<PoolTag> Tags
    {
        get => _tags;
        init => _tags = [..value];
    }

    [NotNull] public List<IMutatorComponent> Mutators { get; } = [];
}

public static class PoolComponentExtensions
{
    public static bool HasTag([NotNull] this PoolComponent pc, PoolTag tag)
    {
        return pc.Tags.Contains(tag);
    }

    public static bool HasAllTags([NotNull] this PoolComponent pc, [NotNull] params PoolTag[] tags)
    {
        return tags.Aggregate(true, (b, tag) => b && pc.Tags.Contains(tag));
    }
    
    public static bool HasAnyTags([NotNull] this PoolComponent pc, [NotNull] params PoolTag[] tags)
    {
        return tags.Aggregate(false, (b, tag) => b || pc.Tags.Contains(tag));
    }
}