using FrogBattleV4.Core.CharacterSystem.Pools;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class PoolCostModifier : BasicModifierComponent<PoolValueCalcContext>
{
    public string PoolKey { get; init; }
    public override bool AppliesInContext(PoolValueCalcContext ctx)
    {
        return ctx.PoolId == PoolKey && ctx.Channel == PoolPropertyChannel.Cost;
    }
}