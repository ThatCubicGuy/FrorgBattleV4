using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Commands;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Abilities.Components;

public class AttackComponent : ShardComponent
{
    public required AmountFormula Formula { get; init; }
    public required DamageMetadata Metadata { get; init; }
    public AttackProperties Properties { get; init; } = AttackProperties.Default;

    public override void GenerateCommands(ShardResolutionScope scope, BattleEnvironment env, CommandBuilder builder)
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
            Flags = Properties.Flags,
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

/// <summary>
/// Defines extra properties for the attack.
/// </summary>
/// <param name="Flags">Damage-related properties.</param>
/// <param name="CanCrit">Whether this attack can crit.</param>
public sealed record AttackProperties(DamageFlags Flags, bool CanCrit = true)
{
    public static readonly AttackProperties Default = new(DamageFlags.None);
}