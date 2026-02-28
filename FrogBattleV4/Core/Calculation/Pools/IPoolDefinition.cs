#nullable enable
using System.Collections.Generic;

namespace FrogBattleV4.Core.Calculation.Pools;

public interface IPoolDefinition
{
    public PoolId Id { get; }
    public IEnumerable<PoolTag> Tags { get; }
    double GetInitialValue(ModifierContext ctx);
    double? GetMinValue(ModifierContext ctx);
    double? GetMaxValue(ModifierContext ctx);
}