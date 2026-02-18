using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.EffectSystem.StatusEffects;

public interface IMutatorComponent
{
    void OnApply(StatusEffectApplicationContext ctx);
    void OnRemove(StatusEffectRemovalContext ctx);
    void OnTurnStart(PoolComponent pool);
    void OnTurnEnd(PoolComponent pool);
}