#nullable enable
using System;
using FrogBattleV4.Core.Extensions;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class SimpleStatModifier : IStatModifier
{
    public required string Stat { get; init; }
    public required double Amount { get; init; }
    public required ModifierOperation Operation { get; init; }

    public double Apply(double currentValue, EffectContext ctx)
    {
        return Operation.Apply(Amount, ctx.Holder.BaseStats[Stat], currentValue);
    }
}