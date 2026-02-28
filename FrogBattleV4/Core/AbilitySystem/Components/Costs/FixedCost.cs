using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.AbilitySystem.Components.Costs;

public class FixedCost : CostComponent
{
    public required PoolId Pool { get; init; }
    public required double BaseAmount { get; init; }
    public PoolMutationFlags CostFlags { get; init; } = PoolMutationFlags.None;

    [Pure]
    public override IEnumerable<MutationCommand> GetCostRequests(AbilityExecContext ctx)
    {
        yield return new MutationCommand(-BaseAmount, MutationSelector.ById(Pool), CostFlags);
    }
}