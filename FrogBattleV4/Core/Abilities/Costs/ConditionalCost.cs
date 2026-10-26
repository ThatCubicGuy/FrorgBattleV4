using System.Collections.Generic;
using FrogBattleV4.Core.Abilities.Requirements;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Abilities.Costs;

public class ConditionalCost : CostComponent
{
    public required System.Func<LinkResolutionState, BattleEnvironment, bool> Predicate { get; init; }
    public required CostComponent CostIfTrue { get; init; }
    public CostComponent CostIfFalse { get; init; } = new SingleCost { Formula = new CostFormula.Flat(0), Pool = PoolName.Mana };

    public override IEnumerable<MutateCommand> GetCost(LinkResolutionState state, BattleEnvironment env)
    {
        return (Predicate(state, env) ? CostIfTrue : CostIfFalse).GetCost(state, env);
    }
}