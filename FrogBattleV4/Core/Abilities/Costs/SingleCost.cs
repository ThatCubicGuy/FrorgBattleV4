using System.Collections.Generic;
using FrogBattleV4.Core.Abilities.Requirements;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Abilities.Costs;

public class SingleCost : CostComponent
{
    public required PoolName Pool { get; init; }

    public required CostFormula Formula { get; init; }

    public override IEnumerable<MutateCommand> GetCost(LinkResolutionState state, BattleEnvironment env)
    {
        var ctx = new RelationContext { Actor = state.User, Target = state.User };
        yield return new MutateCommand
        {
            TotalAmount = new MutateQuery
            {
                BaseValue = Formula.Resolve(state.User, env),
                Pool = Pool,
                Channel = PoolMutChannel.Cost,
                Context = ctx,
            }.Calculate(env),
            Relation = ctx,
            TargetPool = Pool,
            Channel = PoolMutChannel.Cost,
        };
    }
}