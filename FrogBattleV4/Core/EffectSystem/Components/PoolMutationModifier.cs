using System;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.CharacterSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class PoolMutationModifier : IPoolMutationModifier
{
    public required string PoolId { get; init; }
    public required PoolModType Channel { get; init; }
    public required double Amount { get; init; }
    public required ModifierOperation Operation { get; init; }
    public PoolCalcContext Apply(PoolCalcContext ctx)
    {
        if (ctx.PoolId != PoolId) return ctx;
        ctx.Mods = ctx.Mods.Add(Operation, Amount);
        return ctx;
    }
}