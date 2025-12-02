#nullable enable
using System;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class SimpleStatModifier : IStatModifier
{
    public required string Stat { get; init; }
    public required double Amount { get; init; }
    public required ModifierOperation Operation { get; init; }

    public double Apply(double currentValue, StatContext ctx)
    {
        return Operation switch
        {
            ModifierOperation.AddValue => currentValue + Amount,
            ModifierOperation.MultiplyBase => currentValue + Amount * ctx.Holder.BaseStats[Stat],
            ModifierOperation.MultiplyTotal => currentValue * Amount,
            ModifierOperation.Maximum => Math.Max(currentValue, Amount),
            ModifierOperation.Minimum => Math.Min(currentValue, Amount),
            _ => throw new System.InvalidOperationException($"Invalid operator: {Operation}")
        };
    }
}