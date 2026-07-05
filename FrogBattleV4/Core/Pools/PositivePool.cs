using System.Collections.Frozen;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.Pools;

public abstract class PositivePool(PoolId id, double maxValue, params PoolTag[] tags) : IPoolDefinition
{
    public PoolId Id { get; init; } = id;
    public double MaxValue { get; init; } = maxValue;
    public FrozenSet<PoolTag> Tags { get; init; } = tags.ToFrozenSet();
    public abstract double GetInitialValue(ModifierContext ctx);
}