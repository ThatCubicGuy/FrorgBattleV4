using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.AbilitySystem.Components;

public interface ICostComponent
{
    /// <summary>
    /// Gets the base amount of the cost component, with no modifier calculations.
    /// </summary>
    /// <param name="ctx">Context in which to get the base cost.</param>
    /// <returns>Base cost request.</returns>
    [Pure] IEnumerable<Pipelines.Pools.MutationIntent> GetCostRequests(AbilityExecContext ctx);
}