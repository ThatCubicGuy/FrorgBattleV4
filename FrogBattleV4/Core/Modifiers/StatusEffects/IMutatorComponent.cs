using FrogBattleV4.Core.Abilities.Components.Actions;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public interface IMutatorComponent
{
    void OnApply(ApplyEffectAction ctx);
    void OnRemove(RemoveEffectAction ctx);
    void OnTurnStart(PoolContainer pool);
    void OnTurnEnd(PoolContainer pool);
}