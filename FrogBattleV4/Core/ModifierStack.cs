using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace FrogBattleV4.Core;

// This doesn't quite make sense, but I had an idea lmao
// NEVERMIND it's the GREATEST PLAN !!!
public struct ModifierStack()
{
    private double _addValue = 0;
    private double _addBasePercent = 0;
    private double _multiplyTotal = 1;
    private double _minimum = double.MinValue;
    private double _maximum = double.MaxValue;
    
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
    
    /// <summary>
    /// Applies these modifiers to a value.
    /// </summary>
    /// <param name="baseAmount">The starting amount of the value.</param>
    /// <returns>The final computed value.</returns>
    [Pure]
    public double ApplyTo(double baseAmount)
    {
        var total = baseAmount;
        total += this[ModifierOperation.AddValue];
        total += this[ModifierOperation.AddBasePercent] * baseAmount;
        total *= this[ModifierOperation.MultiplyTotal];
        total = Math.Clamp(total,
            this[ModifierOperation.Minimum],
            this[ModifierOperation.Maximum]);
        return total;
    }

    public override string ToString()
    {
        return $"Additive: {_addValue}," +
               $" BasePercent: {_addBasePercent}," +
               $" MultiplyTotal: {_multiplyTotal}," +
               $" Minimum: {_minimum}," +
               $" Maximum: {_maximum}";
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
    /// <summary>
    /// Adds the provided amount to the given operation in <paramref name="modifierStack"/>.
    /// </summary>
    /// <param name="modifierStack">The modifier onto which to add the given value.</param>
    /// <param name="operation">The operation onto which to add the given value.</param>
    /// <param name="amount">The value to compound into the modifier stack.</param>
    /// <returns>A new modifier stack with the applied modification.</returns>
    [Pure]
    public static ModifierStack AddTo(this ModifierStack modifierStack, ModifierOperation operation, double amount)
    {
        modifierStack[operation] = operation switch
        {
            ModifierOperation.Maximum => Math.Min(amount, modifierStack[operation]),
            ModifierOperation.Minimum => Math.Max(amount, modifierStack[operation]),
            ModifierOperation.MultiplyTotal => modifierStack[operation] * amount,
            _ => modifierStack[operation] + amount
        };
        return modifierStack;
    }

    /// <summary>
    /// Combines the modifiers from two separate stacks into one.
    /// </summary>
    /// <param name="mod">The first mod to add.</param>
    /// <param name="other">The mod to add to the first.</param>
    /// <returns>A new modifier with the combined values.</returns>
    [Pure]
    public static ModifierStack AddAll(this ModifierStack mod, ModifierStack other)
    {
        return Enum.GetValuesAsUnderlyingType<ModifierOperation>().Cast<ModifierOperation>()
            .Aggregate(mod, (current, op) => current.AddTo(op, other[op]));
    }
    
    /// <summary>
    /// Combines the modifiers from two separate stacks into one, based on a given ratio.
    /// </summary>
    /// <param name="mod">The first mod to add.</param>
    /// <param name="other">The mod to add to the first.</param>
    /// <param name="ratio">A number between 0 and 1 indicating the ratio at which to combine them.</param>
    /// <returns>A new modifier with the combined values.</returns>
    [Pure]
    public static ModifierStack AddAllWithRatio(this ModifierStack mod, ModifierStack other, double ratio)
    {
        mod[ModifierOperation.AddValue] += other[ModifierOperation.AddValue] * ratio;
        mod[ModifierOperation.AddBasePercent] += other[ModifierOperation.AddBasePercent] * ratio;
        mod[ModifierOperation.MultiplyTotal] *= Math.Pow(other[ModifierOperation.MultiplyTotal], ratio);
        
        // Caps can't really be applied partially, so we only apply them if the ratio is 100% or greater.
        if (ratio < 1) return mod;
        mod[ModifierOperation.Minimum] = Math.Max(mod[ModifierOperation.Minimum], other[ModifierOperation.Minimum]);
        mod[ModifierOperation.Maximum] = Math.Min(mod[ModifierOperation.Maximum], other[ModifierOperation.Maximum]);
        return mod;
    }
}