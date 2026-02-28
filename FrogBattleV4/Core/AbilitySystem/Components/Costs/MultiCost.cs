using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.AbilitySystem.Components.Costs;

public class MultiCost : CostComponent
{
    private readonly List<CostComponent> _costs;
    
    [NotNull]
    public required IEnumerable<CostComponent> Costs
    {
        get => _costs;
        init => _costs = value.ToList();
    }

    [Pure]
    public override IEnumerable<MutationCommand> GetCostRequests(AbilityExecContext ctx)
    {
        return _costs.SelectMany(cc => cc.GetCostRequests(ctx));
    }
}