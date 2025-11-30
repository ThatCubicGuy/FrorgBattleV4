using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.EffectSystem.ActiveEffects;

public interface IMutatorComponent
{
    void OnApply(ActiveEffectContext ctx);
    void OnRemove(ActiveEffectContext ctx);
    void OnTurnStart(TurnContext ctx);
    void OnTurnEnd(TurnContext ctx);
}