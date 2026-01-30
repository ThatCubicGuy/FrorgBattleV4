using System;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core;

// This doesn't quite make sense, but I had an idea lmao
public struct ModifierStack()
{
    private double _addValue = 0;
    private double _addBasePercent = 0;
    private double _multiplyTotal = 1;
    private double _minimum = double.NaN;
    private double _maximum = double.NaN;
    
    public double this[ModifierOperation operation]
    {
        get => this[(int)operation];
        set => this[(int)operation] = value;
    }

    private double this[int index]
    {
        get => index switch
        {
            0 => _addValue,
            1 => _addBasePercent,
            2 => _multiplyTotal,
            3 => _minimum,
            4 => _maximum,
            _ => throw new IndexOutOfRangeException($"Invalid ModifierOperation! ({index})")
        };
        set
        {
            switch (index)
            {
                case 0: _addValue = value; break;
                case 1: _addBasePercent = value; break;
                case 2: _multiplyTotal = value; break;
                case 3: _minimum = value; break;
                case 4: _maximum = value; break;
                default: throw new IndexOutOfRangeException($"Invalid ModifierOperation! ({index})");
            }
        }
    }
}

public enum ModifierOperation
{
    AddValue = 0,
    AddBasePercent = 1,
    MultiplyTotal = 2,
    Minimum = 3,
    Maximum = 4,
}

public static class ModifiersExtensions
{
    [Pure]
    public static ModifierStack NewStack(ModifierOperation operation, double baseAmount)
    {
        return default(ModifierStack).AddTo(operation, baseAmount);
    }
    /// <summary>
    /// Adds the provided amount to the given operation in <paramref name="modifierStack"/>.
    /// </summary>
    /// <param name="modifierStack">The modifier onto which to add </param>
    /// <param name="operation"></param>
    /// <param name="baseAmount"></param>
    /// <returns></returns>
    [Pure]
    public static ModifierStack AddTo(this ModifierStack modifierStack, ModifierOperation operation, double baseAmount)
    {
        modifierStack[operation] = operation switch
        {
            ModifierOperation.Maximum => Math.Min(baseAmount, modifierStack[operation]),
            ModifierOperation.Minimum => Math.Max(baseAmount, modifierStack[operation]),
            ModifierOperation.MultiplyTotal => modifierStack[operation] * baseAmount,
            _ => modifierStack[operation] + baseAmount
        };
        return modifierStack;
    }

    /// <summary>
    /// Combines the modifiers from two separate structs into one.
    /// </summary>
    /// <param name="mod">The first mod to add.</param>
    /// <param name="other">The mod to add to the first.</param>
    /// <returns>A new modifier with the combined values.</returns>
    [Pure]
    public static ModifierStack AddAll(this ModifierStack mod, ModifierStack other)
    {
        mod[ModifierOperation.AddValue] += other[ModifierOperation.AddValue];
        mod[ModifierOperation.AddBasePercent] += other[ModifierOperation.AddBasePercent];
        mod[ModifierOperation.MultiplyTotal] *= other[ModifierOperation.MultiplyTotal];
        mod[ModifierOperation.Minimum] = Math.Min(mod[ModifierOperation.Minimum], other[ModifierOperation.Minimum]);
        mod[ModifierOperation.Maximum] = Math.Max(mod[ModifierOperation.Maximum], other[ModifierOperation.Maximum]);
        return mod;
    }
    /// <summary>
    /// Applies these modifiers to a value.
    /// </summary>
    /// <param name="modifierStack">The modifiers to apply</param>
    /// <param name="baseAmount">The starting amount of the value.</param>
    /// <returns></returns>
    [Pure]
    public static double Apply(this ModifierStack modifierStack, double baseAmount)
    {
        var total = baseAmount;
        total += modifierStack[ModifierOperation.AddValue];
        total += modifierStack[ModifierOperation.AddBasePercent] * baseAmount;
        total *= modifierStack[ModifierOperation.MultiplyTotal];
        total = Math.Clamp(total,
            modifierStack[ModifierOperation.Minimum],
            modifierStack[ModifierOperation.Maximum]);
        return total;
    }
}