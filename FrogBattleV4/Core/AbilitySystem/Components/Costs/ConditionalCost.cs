using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.AbilitySystem.Components.Costs;

public record ConditionalCost(
    [NotNull] Predicate<AbilityExecContext> Condition,
    [NotNull] ICostComponent CostIfTrue,
    ICostComponent CostIfFalse = null) : ICostComponent
{
    [Pure]
    public IEnumerable<MutationIntent> GetCostRequests(AbilityExecContext ctx)
    {
        return Condition(ctx) ? CostIfTrue.GetCostRequests(ctx) : CostIfFalse?.GetCostRequests(ctx) ?? [];
    }
}