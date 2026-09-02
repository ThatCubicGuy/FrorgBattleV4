using System.Collections.Generic;
using FrogBattleV4.Core.Abilities.Components.Requirements;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Actions;

namespace FrogBattleV4.Core.Abilities.Components.Costs;

public class ConditionalCost(IShard parentShard) : CostComponent(parentShard)
{
    public required System.Func<LinkResolutionState, BattleEnvironment, bool> Predicate { get; init; }
    public required CostComponent CostIfTrue { get; init; }
    public CostComponent CostIfFalse { get; init; } = new SingleCost(parentShard) { Formula = new CostFormula.Flat(0), Pool = PoolId.Mana };

    public override IEnumerable<Mutate> GetCost(LinkResolutionState state, BattleEnvironment env)
    {
        return (Predicate(state, env) ? CostIfTrue : CostIfFalse).GetCost(state, env);
    }
}