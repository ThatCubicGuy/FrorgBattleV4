#nullable enable
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FrogBattleV4.Core.Calculation.Pools;

public sealed class PoolComponent
{
    private double _currentValue;
    // Context for calculating capacity is always identical
    private readonly ModifierContext _ctx;

    public PoolComponent(PoolInitContext ctx)
    {
        Definition = ctx.Definition;
        _ctx = new ModifierContext(ctx.Target);
        _currentValue = ctx.Definition.GetInitialValue(_ctx);
    }

    public event Action<PoolComponent, double, double>? ValueChanged;

    public IPoolDefinition Definition { get; }

    public double CurrentValue
    {
        get => _currentValue;
        set
        {
            var old = _currentValue;
            _currentValue = Math.Clamp(value,
                MinValue ?? double.MinValue,
                MaxValue ?? double.MaxValue);
            if (!old.Equals(_currentValue)) ValueChanged?.Invoke(this, old, _currentValue);
        }
    }

    public double? MaxValue => Definition.GetMaxValue(_ctx);
    public double? MinValue => Definition.GetMinValue(_ctx);

    // I like having both '?' and [NotNull] tags in my code, so in
    // this context where I only have one nullable reference field
    // I won't enable file-wide nullable annotations because Rider
    // will give me warnings about redundant [NotNull] attributes.
#nullable disable
    public bool HasTag(PoolTag tag)
    {
        return Definition.Tags.Contains(tag);
    }

    public bool HasAllTags([NotNull] params PoolTag[] tags)
    {
        return tags.All(Definition.Tags.Contains);
    }

    public bool HasAnyTags([NotNull] params PoolTag[] tags)
    {
        return tags.Any(tag => Definition.Tags.Contains(tag));
    }
}