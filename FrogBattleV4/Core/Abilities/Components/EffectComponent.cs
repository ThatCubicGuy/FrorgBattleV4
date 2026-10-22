using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Commands;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Abilities.Components;

public class EffectComponent(IShard parentShard) : ShardComponent(parentShard), IShardCommandComponent
{
    public required EffectChanceFormula Formula { get; init; }
    public required ApplyEffectData Data { get; init; }

    public void GenerateCommands(LinkResolutionState state, BattleEnvironment env, CommandBuilder builder)
    {
        foreach (var targeting in state.Selections)
        {
            GenerateScoped(new ShardResolutionScope(state.User, targeting, state.Modifiers), env, builder);
        }
    }

    private void GenerateScoped(ShardResolutionScope scope, BattleEnvironment env, CommandBuilder builder)
    {
        builder.Emit(new ApplyEffectCommand
        {
            ApplicationChance = Formula.Resolve(scope, env),
            Data = Data,
            Relation = new RelationContext
            {
                Actor = scope.User,
                Target = scope.Targeting.Target,
            },
        });
    }
}

public abstract record EffectChanceFormula
{
    private EffectChanceFormula() { }

    public abstract double Resolve(ShardResolutionScope scope, BattleEnvironment env);

    public sealed record Fixed(double FixedChance) : EffectChanceFormula
    {
        public override double Resolve(ShardResolutionScope scope, BattleEnvironment env)
        {
            return FixedChance;
        }
    }

    public sealed record Base(double BaseChance, ApplyEffectData Data) : EffectChanceFormula
    {
        public override double Resolve(ShardResolutionScope scope, BattleEnvironment env)
        {
            return new EffectChanceQuery
            {
                BaseValue = BaseChance,
                Data = Data,
                Context = new RelationContext
                {
                    Actor = scope.User,
                    Target = scope.Targeting.Target,
                },
            }.Calculate(env);
        }
    }
}