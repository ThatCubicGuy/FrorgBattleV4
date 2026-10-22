using System;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Abilities.Components;

public class AttackComponent(IShard parentShard) : ShardComponent(parentShard), IShardCommandComponent
{
    public required DamageFormula Formula { get; init; }
    public required DamageData Data { get; init; }
    public AttackProperties Properties { get; init; } = new();

    public void GenerateCommands(LinkResolutionState state, BattleEnvironment env, CommandBuilder builder)
    {
        var selections = state.Selections.ToList();
        var split = Properties.SplitDamageEvenly ? selections.Count : 1;
        foreach (var targeting in selections)
        {
            var scope = new ShardResolutionScope(state.User, targeting, state.Modifiers);
            var baseValue = Formula.Resolve(scope, env) / split;
            var ctx = new RelationContext
            {
                Actor = state.User,
                Target = targeting.Target
            };
            builder.Emit(new DamageCommand
            {
                NormalAmount = new DamageQuery
                {
                    BaseValue = baseValue,
                    Data = Data,
                    IsCrit = false,
                    Context = ctx
                }.Calculate(env),
                CriticalAmount = new DamageQuery
                {
                    BaseValue = baseValue,
                    Data = Data,
                    IsCrit = Properties.CanCrit,
                    Context = ctx
                }.Calculate(env),
                CriticalChance = Properties.CanCrit ? new DamageStatQuery
                {
                    Data = Data,
                    Channel = DamageStatChannel.CritRate,
                    Subject = state.User,
                    Reference = targeting.Target
                }.Calculate(env) : 0,
                Data = Data,
                Relation = ctx
            });
        }
    }
}

public sealed record AttackProperties(bool CanCrit = true, bool SplitDamageEvenly = false);

public abstract record DamageFormula
{
    private DamageFormula()
    {
    }

    /// <summary>
    /// A number between 0 and 1 that determines damage falloff per target rank.
    /// A higher number means lower damage to higher rank targets, down to zero.
    /// </summary>
    public required double RankFalloff { get; init; }

    public abstract double Resolve(ShardResolutionScope scope, BattleEnvironment env);
    private double GetFalloffModifier(ShardResolutionScope scope) => Math.Pow(1 - RankFalloff, scope.Targeting.Rank);

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