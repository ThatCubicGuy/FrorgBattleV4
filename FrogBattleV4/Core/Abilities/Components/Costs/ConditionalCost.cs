using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Abilities.Components.Actions;
using FrogBattleV4.Core.Abilities.Components.Requirements;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Components.Costs;

public class ConditionalCost(AbilityShard shard) : CostRequirement(shard)
{
    public required Func<ShardLinkContext, bool> Predicate { get; init; }
    public required CostRequirement CostIfTrue { get; init; }

    public CostRequirement CostIfFalse { get; init; } = new FixedCost(shard) { BaseAmount = 0, Pool = PoolId.Mana };

    [Pure]
    public override IEnumerable<Mutate> GetCost(ShardLinkContext ctx)
    {
        return Predicate(ctx) ? CostIfTrue.GetCost(ctx) : CostIfFalse.GetCost(ctx);
    }
}