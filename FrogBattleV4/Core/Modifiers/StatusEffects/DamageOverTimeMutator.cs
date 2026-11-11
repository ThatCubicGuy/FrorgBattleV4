using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public class DamageOverTimeMutator : IMutatorComponent
{
    // TODO (Nova): Modify DamageFormula namespace
    public required DamageFormula Damage { get; init; }
    public required DamageMetadata Metadata { get; init; }
    public void Tick(TurnMoment moment, StatusEffectInstance effect, BattleEnvironment env, CommandBuilder builder)
    {
        // Damage over time only happens at the start of the turn
        if (moment != TurnMoment.Start) return;

        var damageAmount = Damage.Resolve(new ShardResolutionScope(effect.Applicant, new ShardTargetingContext
        {
            Target = effect.Holder,
            Rank = 0,
        }), env);

        builder.Emit(new DamageCommand
        {
            NormalAmount = damageAmount,
            CriticalAmount = damageAmount,
            CriticalChance = 0,
            Relation = new RelationContext
            {
                Actor = effect.Applicant,
                Target = effect.Holder,
            },
            Metadata = Metadata,
            Data = DamageData.None,
        });
    }
}

// Technically could be fine to just have an attack component...?
file class DoT : IMutatorComponent
{
    public required AttackComponent Attack { get; init; }
    public void Tick(TurnMoment moment, StatusEffectInstance effect, BattleEnvironment env, CommandBuilder builder)
    {
        if (moment == TurnMoment.Start)
        {
            Attack.GenerateCommands(new ShardResolutionScope(effect.Applicant, new ShardTargetingContext
            {
                Target = effect.Holder,
                Rank = 0,
            }), env, builder);
        }
    }
}