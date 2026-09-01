using System;
using FrogBattleV4.Core.Abilities.Components.Commands;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Components.Requirements;

// TODO (Nova): massive rework lmao i'm so bad at this
public abstract class CostRequirement(AbilityShard shard) : ShardComponent(shard), IShardRequirement, IShardCommand
{
    public required CostFormula Formula { get; init; }

    public void Generate(ShardResolutionScope scope, BattleContext env, LinkResolutionBuilder builder)
    {
        foreach (var cost in Formula.Resolve(scope, env))
        {
            builder.Emit(cost);
        }
    }

    public bool IsFulfilled(ShardLinkContext ctx)
    {
        return Formula.Resolve(ctx.User, ctx.Environment).All(cost => cost.Target.Pools[cost.TargetPool].CurrentValue >= cost.TotalAmount);
    }
}

public abstract record CostFormula : IFormula
{
    private CostFormula()
    {
    }
    public abstract double Resolve(ShardResolutionScope scope, BattleContext env);
    public sealed record Flat(double Amount) : CostFormula
    {
        public override double Resolve(ShardResolutionScope scope, BattleContext env)
        {
            return Amount;
        }
    }

    public sealed record StatPercentage(StatId Scalar, double Ratio) : CostFormula
    {
        public override double Resolve(ShardResolutionScope scope, BattleContext env)
        {
            var user = env.GetEntity(scope.User) as Entities.FighterBase
                       ?? throw new InvalidOperationException($"Cannot use StatPercentage formula on member " +
                                                              $"{scope.User} because they have no stats");
            return user.GetStat(Scalar, scope.Targeting.Target, env) * Ratio;
        }
    }
}