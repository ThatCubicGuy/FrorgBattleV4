using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Commands;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Abilities.Components;

public class EffectComponent : IShardCommandComponent
{
    public required EffectChanceFormula Chance { get; init; }
    public required ApplyEffectData Data { get; init; }

    public void GenerateCommands(ShardResolutionScope scope, BattleEnvironment env, CommandBuilder builder)
    {
        builder.Emit(new ApplyEffectCommand
        {
            ApplicationChance = Chance.Resolve(scope, Data, env),
            Data = Data,
            Relation = new RelationContext
            {
                Actor = scope.User,
                Target = scope.Targeting.Target,
            },
        });
    }
}

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(Base), "base")]
[JsonDerivedType(typeof(Fixed), "fixed")]
public abstract record EffectChanceFormula
{
    private EffectChanceFormula() { }

    public abstract double Resolve(ShardResolutionScope scope, ApplyEffectData data, BattleEnvironment env);

    public sealed record Fixed(double Chance) : EffectChanceFormula
    {
        public override double Resolve(ShardResolutionScope scope, ApplyEffectData data, BattleEnvironment env)
        {
            return Chance;
        }
    }

    public sealed record Base(double Chance) : EffectChanceFormula
    {
        public override double Resolve(ShardResolutionScope scope, ApplyEffectData data, BattleEnvironment env)
        {
            return new EffectChanceQuery
            {
                BaseValue = Chance,
                Data = data,
                Context = new RelationContext
                {
                    Actor = scope.User,
                    Target = scope.Targeting.Target,
                },
            }.Calculate(env);
        }
    }
}