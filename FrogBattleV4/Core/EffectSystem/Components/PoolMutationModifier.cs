using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class PoolMutationModifier : BasicModifierComponent<PoolValueCalcContext>
{
    public required string PoolId { get; init; }
    public required PoolPropertyChannel Channel { get; init; }
    public override bool AppliesInContext(PoolValueCalcContext ctx)
    {
        return ctx.PoolId == PoolId;
    }
}