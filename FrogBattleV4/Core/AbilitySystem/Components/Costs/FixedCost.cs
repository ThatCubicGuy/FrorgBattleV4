using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.CharacterSystem.Pools;

namespace FrogBattleV4.Core.AbilitySystem.Components.Costs;

public record FixedCost(
    [NotNull] string Pool,
    double BaseAmount,
    PoolMutationFlags CostFlags = PoolMutationFlags.None) : ICostComponent
{
    [Pure]
    public IEnumerable<MutationRequest> GetMutationRequests(AbilityExecContext ctx)
    {
        return [MutationRequestBuilder.ById(Pool, -BaseAmount, CostFlags)];
    }
}