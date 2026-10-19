using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public class DamageOverTimeMutator : IMutatorComponent
{
    public required double DamageAmount { get; init; }
    public required DamageData Data { get; init; }
    public void Tick(TurnMoment moment, StatusEffectInstance effect, CommandBuilder builder)
    {
        builder.Emit(new DamageCommand
        {
            TotalAmount = DamageAmount,
            IsCrit = false,
            Relation = new RelationContext
            {
                Actor = effect.Applicant,
                Target = effect.Holder
            },
            Data = Data
        });
    }
}