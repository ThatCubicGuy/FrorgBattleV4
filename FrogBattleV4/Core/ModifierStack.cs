using System;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core;

// BAHHAHAHA THIS IS LITERALLY A VECTOR
// YOU CAN ADD IT TO ANOTHER
// YOU CAN MULTIPLY BY SCALAR
// DUDE I CAN MAKE
// VECTOR SPACES
// OUT OF MODIFIER STACKS
// AND COMPUTE THE BEST POSSIBLE COMBINATION OF THEM ???
// I CAN * APPLY * LINEAR ALGEBRA ???
public struct ModifierStack()
{
    public double AddValue { get; set; } = 0;
    public double AddBasePercent { get; set; } = 0;
    public double MultiplyTotal { get; set; } = 1;
    public double Minimum { get; set; } = double.MinValue;
    public double Maximum { get; set; } = double.MaxValue;

    public double this[ModifierOperation operation]
    {
        get => this[(int)operation];
        set => this[(int)operation] = value;
    }

    private double this[int index]
    {
        get => index switch
        {
            0 => AddValue,
            1 => AddBasePercent,
            2 => MultiplyTotal,
            3 => Minimum,
            4 => Maximum,
            _ => throw new IndexOutOfRangeException($"Invalid ModifierOperation! ({index})")
        };
        set
        {
            switch (index)
            {
                case 0: AddValue = value; break;
                case 1: AddBasePercent = value; break;
                case 2: MultiplyTotal = value; break;
                case 3: Minimum = value; break;
                case 4: Maximum = value; break;
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
        return $"Additive: {AddValue}," +
               $" BasePercent: {AddBasePercent}," +
               $" MultiplyTotal: {MultiplyTotal}," +
               $" Minimum: {Minimum}," +
               $" Maximum: {Maximum}";
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
    /// Combines the modifiers from two separate stacks into one.
    /// </summary>
    /// <param name="mod">The first mod to add.</param>
    /// <param name="other">The mod to add to the first.</param>
    /// <returns>A new modifier with the combined values.</returns>
    [Pure]
    public static ModifierStack Add(this ModifierStack mod, ModifierStack other)
    {
        mod.AddValue += other.AddValue;
        mod.AddBasePercent += other.AddBasePercent;
        mod.MultiplyTotal *= other.MultiplyTotal;

        mod.Minimum = Math.Max(mod.Minimum, other.Minimum);
        mod.Maximum = Math.Min(mod.Maximum, other.Maximum);

        return mod;
    }

    /// <summary>
    /// Multiplies a modifier stack by a scalar.
    /// </summary>
    /// <param name="mod">Modifier to scale.</param>
    /// <param name="scalar">Real value to scale by.</param>
    /// <returns>A scaled modifier result.</returns>
    [Pure]
    public static ModifierStack MultiplyBy(this ModifierStack mod, double scalar)
    {
        mod.AddValue *= scalar;
        mod.AddBasePercent *= scalar;
        mod.MultiplyTotal = Math.Pow(mod.MultiplyTotal, scalar);

        // Completely clueless as to how I'd modify min/max by a scalar... 
        return mod;
    }
}