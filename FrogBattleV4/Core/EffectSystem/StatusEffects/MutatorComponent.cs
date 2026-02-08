using System;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.EffectSystem.StatusEffects;

public class MutatorComponent : IMutatorComponent
{
    public void OnApply(StatusEffectApplicationContext ctx)
    {
        if (ctx.Target is not IHasPools target) return;
    }

    public void OnRemove(StatusEffectRemovalContext ctx)
    {
        throw new NotImplementedException();
    }

    public void OnTurnStart(TurnContext ctx)
    {
        throw new NotImplementedException();
    }

    public void OnTurnEnd(TurnContext ctx)
    {
        throw new NotImplementedException();
    }
}