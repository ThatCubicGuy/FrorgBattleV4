using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.AbilitySystem.Components.Costs;

public class ConditionalCost : CostComponent
{
    [NotNull] public required Func<AbilityExecContext, bool> Predicate { get; init; }
    [NotNull] public required CostComponent CostIfTrue { get; init; }
    public CostComponent CostIfFalse { get; init; }

    [Pure]
    public override IEnumerable<MutationCommand> GetCostRequests(AbilityExecContext ctx)
    {
        return Predicate(ctx) ? CostIfTrue.GetCostRequests(ctx) : CostIfFalse?.GetCostRequests(ctx) ?? [];
    }
}