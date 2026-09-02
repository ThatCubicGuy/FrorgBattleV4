using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Actions;

namespace FrogBattleV4.Core.Abilities.Components.Requirements;

// TODO (Nova): massive rework lmao i'm so bad at this
public abstract class CostComponent(IShard parentShard) : ShardComponent(parentShard), IShardRequirement
{
    public abstract IEnumerable<Mutate> GetCost(LinkResolutionState state, BattleEnvironment env);

    public void GenerateFulfill(LinkResolutionState state, BattleEnvironment env, LinkResolutionBuilder builder)
    {
        foreach (var cost in GetCost(state, env))
        {
            builder.Emit(cost);
        }
    }

    public bool IsFulfilled(LinkResolutionState state, BattleEnvironment env)
    {
        return GetCost(state, env).All(cost =>
            !(cost.TotalAmount > env.GetPoolValue(cost.Relation.Target, cost.Data.TargetPool)));
    }
}

public abstract record CostFormula
{
    private CostFormula()
    {
    }

    public abstract double Resolve(EntityUid user, BattleEnvironment env);
    public sealed record Flat(double Amount) : CostFormula
    {
        public override double Resolve(EntityUid user, BattleEnvironment env)
        {
            return Amount;
        }
    }

    public sealed record StatPercentage(StatId Scalar, double Ratio) : CostFormula
    {
        public override double Resolve(EntityUid user, BattleEnvironment env)
        {
            return new StatQuery
            {
                Stat = Scalar,
                Subject = user,
            }.Calculate(env) * Ratio;
        }
    }

    public sealed record PoolMaxValuePercentage(PoolId Pool, double Percentage) : CostFormula
    {
        public override double Resolve(EntityUid user, BattleEnvironment env)
        {
            return new PoolStatQuery
            {
                PoolId = Pool,
                Channel = PoolValueChannel.Max,
                Subject = user,
            }.Calculate(env) * Percentage;
        }
    }
}