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

    public StatContext Apply(StatContext ctx)
    {
        if (ctx.Stat != Stat) return ctx;
        ctx.ModifierValues[(int)Operation] = Operation switch
        {
            ModifierOperation.Maximum => Math.Max(Amount, ctx.ModifierValues[(int)Operation]),
            ModifierOperation.Minimum => Math.Min(Amount, ctx.ModifierValues[(int)Operation]),
            _ => ctx.ModifierValues[(int)Operation]
        };
        ctx.ModifierValues[(int)Operation] += Amount;
        return ctx;
    }
}