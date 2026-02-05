using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.EffectSystem.StatusEffects;

public interface IMutatorComponent
{
    void OnApply(StatusEffectApplicationContext ctx);
    void OnRemove(StatusEffectRemovalContext ctx);
    void OnTurnStart(TurnContext ctx);
    void OnTurnEnd(TurnContext ctx);
}