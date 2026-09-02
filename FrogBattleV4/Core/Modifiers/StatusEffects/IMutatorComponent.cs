using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Actions;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public interface IMutatorComponent
{
    void OnApply(ApplyEffect ctx);
    void OnRemove(RemoveEffect ctx);
    void OnTurnStart(PoolId pool);
    void OnTurnEnd(PoolId pool);
}