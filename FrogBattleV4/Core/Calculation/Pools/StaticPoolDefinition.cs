#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Calculation.Pools;

public class StaticPoolDefinition : IPoolDefinition
{
    private readonly HashSet<PoolTag> _tags = [];

    public required PoolId Id { get; init; }

    public IEnumerable<PoolTag> Tags
    {
        get => _tags;
        init => _tags = value.ToHashSet();
    }

    public double InitialValue { get; init; } = 0;
    public double? MinValue { get; init; }
    public double? MaxValue { get; init; }

    public double GetInitialValue(ModifierContext ctx) => InitialValue;

    double? IPoolDefinition.GetMinValue(ModifierContext ctx)
    {
        if (!MinValue.HasValue) return null;
        return new PoolValueQuery
        {
            Channel = PoolValueChannel.Max,
            PoolId = Id
        }.Compute(MinValue.Value, ctx);
    }

    double? IPoolDefinition.GetMaxValue(ModifierContext ctx)
    {
        if (!MaxValue.HasValue) return null;
        return new PoolValueQuery
        {
            Channel = PoolValueChannel.Max,
            PoolId = Id
        }.Compute(MaxValue.Value, ctx);
    }
}