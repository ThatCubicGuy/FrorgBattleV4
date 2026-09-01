using System.Linq;
using FrogBattleV4.Core.Abilities.Components.Actions;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Components;

public class AttackCommand(AbilityShard parentShard) : ShardComponent(parentShard), IShardCommand
{
    public required DamageFormula Formula { get; init; }
    public required DamageType Type { get; init; }
    public required AttackProperties AttackProperties { get; init; }

    /// <summary>
    /// A number between 0 and 1 that determines damage falloff for subsequent targets hit by a blast attack.
    /// </summary>
    public double Falloff { get; init; }

    public void Resolve(AbilityTargetingContext targeting, LinkResolutionBuilder builder)
    {
        builder.Add(new DealDamage
        {
            TotalAmount = Formula.Resolve(),
            Target = targeting.Target,
            Type = Type,
            Source = AttackProperties.AttackType,
            Crit = ???,
        });
    }
}

public abstract record DamageFormula
{
    public abstract double Resolve(ShardLinkContext context, AbilityTargetingContext targeting);

    public record Flat(double Amount) : DamageFormula
    {
        public override double Resolve(ShardLinkContext context, AbilityTargetingContext targeting)
        {
            return Amount;
        }
    }

    public record StatPercentage(StatId Scalar, double Ratio) : DamageFormula
    {
        public override double Resolve(ShardLinkContext context, AbilityTargetingContext targeting)
        {
            context.Calculate
        }
    }
}

/// <summary>
/// Lightweight record for storing properties related to attacks.
/// </summary>
/// <param name="CanCrit">Whether this attack can generate critical hits.</param>
public record AttackProperties(bool CanCrit, DamageSource AttackType);