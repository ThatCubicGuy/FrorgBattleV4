using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Effects.StatusEffects;

public interface IMutatorComponent
{
    void OnApply(StatusEffectApplicationContext ctx);
    void OnRemove(StatusEffectRemovalContext ctx);
    void OnTurnStart(PoolContainer pool);
    void OnTurnEnd(PoolContainer pool);
}