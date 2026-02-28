using System.Collections.Generic;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.AbilitySystem.Components;

public abstract class CostComponent : IAbilityCommandComponent
{
    /// <summary>
    /// Gets the base amount of the cost component, with no modifier calculations.
    /// </summary>
    /// <param name="ctx">Context in which to get the base cost.</param>
    /// <returns>Base cost request.</returns>
    [Pure]
    public abstract IEnumerable<MutationCommand> GetCostRequests(AbilityExecContext ctx);

    IEnumerable<IBattleCommand> IAbilityCommandComponent.GetContribution(AbilityExecContext ctx)
    {
        return (GetCostRequests(ctx));
    }
}