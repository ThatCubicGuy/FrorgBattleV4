#nullable enable
using System;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.Extensions;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class SimpleStatModifier : IStatModifier
{
    public required string Stat { get; init; }
    public required double Amount { get; init; }
    public required ModifierOperation Operation { get; init; }

    public StatCalcContext Apply(StatCalcContext ctx)
    {
        if (ctx.Stat != Stat) return ctx;
        ctx.Mods[Operation] = Operation switch
        {
            ModifierOperation.Maximum => Math.Min(Amount, ctx.Mods[Operation]),
            ModifierOperation.Minimum => Math.Max(Amount, ctx.Mods[Operation]),
            ModifierOperation.MultiplyTotal => ctx.Mods[Operation] * Amount,
            _ => ctx.Mods[Operation] + Amount
        };
        return ctx;
    }
}