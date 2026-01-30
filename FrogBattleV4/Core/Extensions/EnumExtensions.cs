using System;

namespace FrogBattleV4.Core.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Apply the operator for an amount, considering a base value and a current value.
    /// </summary>
    /// <param name="op">The operator to apply.</param>
    /// <param name="amount">The amount to modify by.</param>
    /// <param name="baseValue">The base value of the item to be modified.</param>
    /// <param name="currentValue">The current value of the item to be modified.</param>
    /// <returns>The modified value.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static void Apply(this ModifierOperation op, double amount, double baseValue, ref double currentValue)
    {
        currentValue = op switch
        {
            ModifierOperation.AddValue => currentValue + amount,
            ModifierOperation.AddBasePercent => currentValue + amount * baseValue,
            ModifierOperation.MultiplyTotal => currentValue * amount,
            ModifierOperation.Maximum => Math.Max(currentValue, amount),
            ModifierOperation.Minimum => Math.Min(currentValue, amount),
            _ => throw new InvalidOperationException($"Invalid operator: {op}")
        };
    }
}