using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.EffectSystem.StatusEffects;

namespace FrogBattleV4.Core.Calculation.Pools;

public class PoolComponent
{
    private double _currentValue;
    private readonly HashSet<PoolTag> _tags = [];
    private readonly List<IMutatorComponent> _mutators = [];
    // I like having both '?' and [NotNull] tags in my code, so in
    // this context where I only have one nullable reference field
    // I won't enable file-wide nullable annotations because Rider
    // will give me warnings about redundant [NotNull] attributes.
    #nullable enable
    public event Action<PoolComponent, double, double>? ValueChanged;
    #nullable disable
    public required PoolId Id { get; init; }

    public double CurrentValue
    {
        get => _currentValue;
        set
        {
            var old = _currentValue;
            _currentValue = value;
            if (MinValue.HasValue) _currentValue = Math.Max(_currentValue, MinValue.Value);
            if (MaxValue.HasValue) _currentValue = Math.Min(_currentValue, MaxValue.Value);
            if (!old.Equals(_currentValue)) ValueChanged?.Invoke(this, old, _currentValue);
        }
    }

    public virtual double? MaxValue { get; init; }
    public virtual double? MinValue { get; init; }

    [NotNull] public IEnumerable<PoolTag> Tags
    {
        get => _tags;
        init => _tags = value.ToHashSet();
    }

    [NotNull]
    public IEnumerable<IMutatorComponent> Mutators
    {
        get => _mutators;
        init => _mutators = value.ToList();
    }

    public void AddMutator(IMutatorComponent mutator) => _mutators.Add(mutator);
}

public static class PoolComponentExtensions
{
    public static bool HasTag([NotNull] this PoolComponent pc, PoolTag tag)
    {
        return pc.Tags.Contains(tag);
    }

    public static bool HasAllTags([NotNull] this PoolComponent pc, [NotNull] params PoolTag[] tags)
    {
        return tags.All(pc.Tags.Contains);
    }

    public static bool HasAnyTags([NotNull] this PoolComponent pc, [NotNull] params PoolTag[] tags)
    {
        return tags.Any(tag => pc.Tags.Contains(tag));
    }
}