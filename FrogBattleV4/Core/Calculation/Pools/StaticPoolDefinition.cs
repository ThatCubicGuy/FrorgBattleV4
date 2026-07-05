#nullable enable
using System.Collections.Frozen;

namespace FrogBattleV4.Core.Calculation.Pools;

public class StaticPoolDefinition : IPoolDefinition
{
    private readonly FrozenSet<PoolTag> _tags = [];

    public required PoolId Id { get; init; }

    public FrozenSet<PoolTag> Tags
    {
        get => _tags;
        init => _tags = value.ToFrozenSet();
    }

    public double InitialValue { get; init; } = 0;
    public double MaxValue { get; init; }

    public double GetInitialValue(ModifierContext ctx) => InitialValue;
}