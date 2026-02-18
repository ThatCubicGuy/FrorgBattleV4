using System;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.EffectSystem.StatusEffects;

public class MutatorComponent : IMutatorComponent
{
    public void OnApply(StatusEffectApplicationContext ctx)
    {
        throw new NotImplementedException();
    }

    public void OnRemove(StatusEffectRemovalContext ctx)
    {
        throw new NotImplementedException();
    }

    public void OnTurnStart(PoolComponent pool)
    {
        throw new NotImplementedException();
    }

    public void OnTurnEnd(PoolComponent pool)
    {
        throw new NotImplementedException();
    }
}