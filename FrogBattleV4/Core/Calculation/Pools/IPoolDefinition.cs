#nullable enable
using System.Collections.Frozen;

namespace FrogBattleV4.Core.Calculation.Pools;

public interface IPoolDefinition
{
    public PoolId Id { get; }
    double MaxValue { get; }
    public FrozenSet<PoolTag> Tags { get; }
    double GetInitialValue(ModifierContext ctx);
}