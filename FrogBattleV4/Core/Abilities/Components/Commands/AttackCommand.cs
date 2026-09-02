using System;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Actions;

namespace FrogBattleV4.Core.Abilities.Components.Commands;

public class AttackCommand(IShard parentShard) : ShardComponent(parentShard), IShardCommand
{
    public required DamageFormula Formula { get; init; }
    public required DamageData Data { get; init; }
    public bool CanCrit { get; init; } = true;

    public void Generate(ShardResolutionScope scope, BattleEnvironment env, LinkResolutionBuilder builder)
    {
        var ctx = new RelationContext
        {
            Actor = scope.User,
            Target = scope.Targeting.Target
        };
        var res = CanCrit ? new CritResolution(new DamageStatQuery
        {
            Data = Data,
            Channel = DamageStatChannel.CritRate,
            Subject = scope.User,
            Reference = scope.Targeting.Target
        }.Calculate(env), env.NextRoll()) : new CritResolution(0, 0);
        builder.Emit(new Damage
        {
            TotalAmount = new DamageQuery
            {
                BaseValue = Formula.Resolve(scope, env),
                Data = Data,
                CritData = res,
                Context = ctx
            }.Calculate(env),
            Data = Data,
            CritData = res,
            Relation = ctx
        });
    }
}

public abstract record DamageFormula
{
    private DamageFormula()
    {
    }

    /// <summary>
    /// A number between 0 and 1 that determines damage falloff per target rank.
    /// A higher number means lower damage to higher rank targets, down to zero.
    /// </summary>
    public required double Falloff { get; init; }

    private double GetFalloffModifier(ShardResolutionScope scope) => Math.Pow(1 - Falloff, scope.Targeting.Rank);
    public abstract double Resolve(ShardResolutionScope scope, BattleEnvironment env);

    public sealed record Flat(double Amount) : DamageFormula
    {
        public override double Resolve(ShardResolutionScope scope, BattleEnvironment env)
        {
            return Amount * GetFalloffModifier(scope);
        }
    }

    public sealed record StatPercentage(StatId Scalar, double Ratio) : DamageFormula
    {
        public override double Resolve(ShardResolutionScope scope, BattleEnvironment env)
        {
            return new StatQuery
            {
                Stat = Scalar,
                Subject = scope.User,
                Reference = scope.Targeting.Target,
            }.Calculate(env) * Ratio * GetFalloffModifier(scope);
        }
    }

    public sealed record PoolValuePercentage(PoolId Pool, double Ratio) : DamageFormula
    {
        public override double Resolve(ShardResolutionScope scope, BattleEnvironment env)
        {
            return env.GetPoolValue(scope.User, Pool) * Ratio * GetFalloffModifier(scope);
        }
    }
}