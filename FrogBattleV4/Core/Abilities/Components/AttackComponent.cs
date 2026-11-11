using System;
using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Commands;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Abilities.Components;

public class AttackComponent : IShardCommandComponent
{
    public required DamageFormula Formula { get; init; }
    public required DamageMetadata Metadata { get; init; }
    public AttackProperties Properties { get; init; } = new(DamageData.None);

    public void GenerateCommands(ShardResolutionScope scope, BattleEnvironment env, CommandBuilder builder)
    {
        var baseValue = Formula.Resolve(scope, env);
        var ctx = new RelationContext
        {
            Actor = scope.User,
            Target = scope.Targeting.Target,
        };
        builder.Emit(new DamageCommand
        {
            NormalAmount = GetDamageValue(baseValue, false, env, ctx),
            CriticalAmount = GetDamageValue(baseValue, true, env, ctx),
            CriticalChance = GetCritChance(env, ctx),
            Metadata = Metadata,
            Data = Properties.Data,
            Relation = ctx,
        });
    }

    private double GetDamageValue(double baseValue, bool isCrit, BattleEnvironment env, in RelationContext ctx)
    {
        return new DamageQuery
        {
            BaseValue = baseValue,
            Metadata = Metadata,
            IsCrit = isCrit,
            Context = ctx,
        }.Calculate(env) * GetDefMultiplier(env, ctx) * (isCrit ? GetCritMultiplier(env, ctx) : 1);
    }

    private static double GetDefMultiplier(BattleEnvironment env, in RelationContext ctx)
    {
        return 3000 / double.Max(3000, 3000 + new StatQuery
        {
            Stat = StatName.Def,
            Subject = ctx.Target,
            Reference = ctx.Actor,
        }.Calculate(env));
    }

    private static double GetCritMultiplier(BattleEnvironment env, in RelationContext ctx)
    {
        return 1 + new StatQuery
        {
            Stat = StatName.CritDamage,
            Subject = ctx.Actor,
            Reference = ctx.Target,
        }.Calculate(env);
    }

    private double GetCritChance(BattleEnvironment env, in RelationContext ctx)
    {
        return Properties.CanCrit
            ? new DamageStatQuery
            {
                Metadata = Metadata,
                Channel = DamageStatChannel.CritRate,
                Subject = ctx.Actor,
                Reference = ctx.Target,
            }.Calculate(env) + new StatQuery
            {
                Stat = StatName.CritRate,
                Subject = ctx.Actor,
                Reference = ctx.Target,
            }.Calculate(env)
            : 0;
    }
}

public sealed record AttackProperties(DamageData Data, bool CanCrit = true);

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Flat), "flat")]
[JsonDerivedType(typeof(StatPercentage), "stat_percent")]
[JsonDerivedType(typeof(PoolValuePercentage), "pool_value_percent")]
public abstract class DamageFormula
{
    private DamageFormula() { }

    /// <summary>
    /// A number between 0 and 1 that determines damage falloff per target rank.
    /// A higher number means lower damage to higher rank targets, down to zero.
    /// </summary>
    public required double RankFalloff { get; init; }

    public abstract double Resolve(ShardResolutionScope scope, BattleEnvironment env);

    private double GetFalloffModifier(ShardResolutionScope scope) => Math.Pow(1 - RankFalloff, scope.Targeting.Rank);

    public sealed class Flat : DamageFormula
    {
        public required double Amount { get; init; }
        public override double Resolve(ShardResolutionScope scope, BattleEnvironment env)
        {
            return Amount * GetFalloffModifier(scope);
        }
    }

    public sealed class StatPercentage : DamageFormula
    {
        public required StatName Scalar { get; init; }
        public required double Ratio { get; init; }
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

    public sealed class PoolValuePercentage : DamageFormula
    {
        public required PoolName Pool { get; init; }
        public required double Ratio { get; init; }
        public override double Resolve(ShardResolutionScope scope, BattleEnvironment env)
        {
            return env.GetPoolValue(scope.User, Pool) * Ratio * GetFalloffModifier(scope);
        }
    }
}