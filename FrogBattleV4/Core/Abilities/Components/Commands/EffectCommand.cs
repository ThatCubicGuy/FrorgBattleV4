using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Actions;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Abilities.Components.Commands;

public class EffectCommand(IShard parentShard) : ShardComponent(parentShard), IShardCommand
{
    public required EffectChanceFormula Formula { get; init; }
    public required ApplyEffectData Data { get; init; }

    public void Generate(LinkResolutionState state, BattleEnvironment env, LinkResolutionBuilder builder)
    {
        foreach (var targeting in state.Selections)
        {
            GenerateScoped(new ShardResolutionScope(state.User, targeting, state.Modifiers), env, builder);
        }
    }

    private void GenerateScoped(ShardResolutionScope scope, BattleEnvironment env, LinkResolutionBuilder builder)
    {
        var res = new EffectChanceResolution(Formula.Resolve(scope, env), env.NextRoll());
        if (res.CanApply)
        {
            builder.Emit(new ApplyEffect
            {
                Data = Data,
                Relation = new RelationContext
                {
                    Actor = scope.User,
                    Target = scope.Targeting.Target,
                },
            });
        }
    }
}

public abstract record EffectChanceFormula
{
    private EffectChanceFormula()
    {
    }
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