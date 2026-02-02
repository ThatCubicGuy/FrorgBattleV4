using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.CharacterSystem.Pools;

namespace FrogBattleV4.Core.AbilitySystem.Components.Costs;

public record SplitCost(
    [NotNull] Func<AbilityExecContext, int> Selector,
    [NotNull] params ICostComponent[] CostList) : ICostComponent
{
    [Pure]
    public IEnumerable<MutationRequest> GetMutationRequests(AbilityExecContext ctx)
    {
        return CostList[Selector(ctx)].GetMutationRequests(ctx);
    }
}