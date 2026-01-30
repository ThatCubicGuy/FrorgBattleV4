using System;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.CharacterSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class PoolMutationModifier : BasicModifierComponent<PoolCalcContext>, IPoolMutationModifier
{
    public required string PoolId { get; init; }
    public required PoolModType Channel { get; init; }
    public override bool AppliesInContext(PoolCalcContext ctx)
    {
        return ctx.PoolId == PoolId;
    }
}