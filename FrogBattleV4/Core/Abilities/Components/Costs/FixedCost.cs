using System.Collections.Generic;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Abilities.Components.Actions;
using FrogBattleV4.Core.Abilities.Components.Requirements;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Components.Costs;

public class FixedCost(AbilityShard shard) : CostRequirement(shard)
{
    public required PoolId Pool { get; init; }
    public required double BaseAmount { get; init; }
    public PoolMutationFlags CostFlags { get; init; } = PoolMutationFlags.None;

    [Pure]
    public override IEnumerable<Mutate> GetCost(ShardLinkContext ctx)
    {
        yield return new Mutate
        {
            TotalAmount = MutationPipeline.Calculate(ctx.User, ctx.SelectedTarget, -1 * BaseAmount),
            Target = ctx.User,
            TargetPool = Pool,
        };
    }
}