using System;
using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities;

/// <summary>
/// Defines a generic formula that can be used for various components to determine a variable base amount.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Flat), "flat")]
[JsonDerivedType(typeof(StatPercentage), "stat_percent")]
[JsonDerivedType(typeof(PoolValuePercentage), "pool_value_percent")]
public abstract class AmountFormula
{
    private AmountFormula() { }

    /// <summary>
    /// A number between 0 and 1 that determines damage falloff per target rank.
    /// A higher number means lower damage to higher rank targets, down to zero.
    /// </summary>
    public required double RankFalloff { get; init; }

    public abstract double Resolve(ShardResolutionScope scope, BattleEnvironment env);

    private double GetFalloffModifier(ShardResolutionScope scope) => Math.Pow(1 - RankFalloff, scope.Targeting.Rank);

    public sealed class Flat : AmountFormula
    {
        public required double Amount { get; init; }
        public override double Resolve(ShardResolutionScope scope, BattleEnvironment env)
        {
            return Amount * GetFalloffModifier(scope);
        }
    }

    public sealed class StatPercentage : AmountFormula
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

    public sealed class PoolValuePercentage : AmountFormula
    {
        public required PoolName Pool { get; init; }
        public required double Ratio { get; init; }
        public override double Resolve(ShardResolutionScope scope, BattleEnvironment env)
        {
            return env.GetPoolValue(scope.User, Pool) * Ratio * GetFalloffModifier(scope);
        }
    }
}