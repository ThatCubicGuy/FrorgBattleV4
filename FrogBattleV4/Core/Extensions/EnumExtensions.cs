using System;
using System.ComponentModel;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;

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
    public static double Apply(this ModifierOperation op, double amount, double baseValue, double currentValue)
    {
        return op switch
        {
            ModifierOperation.AddValue => currentValue + amount,
            ModifierOperation.AddBasePercent => currentValue + amount * baseValue,
            ModifierOperation.MultiplyTotal => currentValue * amount,
            ModifierOperation.Maximum => Math.Max(currentValue, amount),
            ModifierOperation.Minimum => Math.Min(currentValue, amount),
            _ => throw new InvalidOperationException($"Invalid operator: {op}")
        };
    }

    public static bool CanApply(this EffectApplicationContext effect)
    {
        var totalChance = effect.ChanceType switch
        {
            ChanceType.Fixed => effect.ApplicationChance,
            ChanceType.Base => effect.ApplicationChance +
                               effect.Source.GetStat(nameof(Stat.EffectHitRate), effect.Target as IHasStats) -
                               ((effect.Target as IHasStats)?.GetStat(nameof(Stat.EffectRes), effect.Source) ?? 0),
            _ => throw new InvalidEnumArgumentException($"Invalid chance type: {effect.ChanceType}")
        };

        return effect.Rng.NextDouble() < totalChance;
    }
}