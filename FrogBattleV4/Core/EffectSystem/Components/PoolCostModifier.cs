using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.CharacterSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class PoolCostModifier : BasicModifierComponent<PoolCalcContext>
{
    public string PoolKey { get; init; }
    public override bool AppliesInContext(PoolCalcContext ctx)
    {
        return ctx.PoolId == PoolKey && ctx.Channel == PoolModType.Cost;
    }
}