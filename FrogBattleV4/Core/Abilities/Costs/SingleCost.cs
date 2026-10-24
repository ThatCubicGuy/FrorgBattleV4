using System.Collections.Generic;
using FrogBattleV4.Core.Abilities.Requirements;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Abilities.Costs;

public class SingleCost(IShard parentShard) : CostComponent()
{
    // Always set by required property initializer
    private readonly MutateData _data = null!;

    public required PoolName Pool
    {
        get => _data.TargetPool;
        init => _data = new MutateData(value, PoolMutChannel.Cost);
    }

    public required CostFormula Formula { get; init; }

    public override IEnumerable<MutateCommand> GetCost(LinkResolutionState state, BattleEnvironment env)
    {
        var ctx = new RelationContext { Actor = state.User, Target = state.User };
        yield return new MutateCommand
        {
            TotalAmount = new MutateQuery
            {
                BaseValue = Formula.Resolve(state.User, env),
                Data = _data,
                Context = ctx
            }.Calculate(env),
            Data = _data,
            Relation = ctx
        };
    }
}