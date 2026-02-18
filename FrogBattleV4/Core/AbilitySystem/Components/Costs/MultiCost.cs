using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.AbilitySystem.Components.Costs;

public record MultiCost(
    [NotNull] params ICostComponent[] Costs) : ICostComponent
{
    [Pure]
    public IEnumerable<MutationIntent> GetCostRequests(AbilityExecContext ctx)
    {
        return Costs.SelectMany(cc => cc.GetCostRequests(ctx));
    }
}