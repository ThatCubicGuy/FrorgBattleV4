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

    /// <summary>
    /// Returns a copy of this ModifierStack where
    /// all fields have a positive or neutral effect.
    /// </summary>
    /// <returns>A new ModifierStack.</returns>
    [Pure]
    public ModifierStack AsPositive()
    {
        return new ModifierStack
        {
            AddValue = Math.Max(0, AddValue),
            AddBasePercent = Math.Max(0, AddBasePercent),
            MultiplyTotal = Math.Max(1, MultiplyTotal),
            Minimum = Minimum,
            Maximum = Maximum,
        };
    }

    /// <summary>
    /// Returns a copy of this ModifierStack where
    /// all fields have a negative or neutral effect.
    /// </summary>
    /// <returns>A new ModifierStack.</returns>
    [Pure]
    public ModifierStack AsNegative()
    {
        return new ModifierStack
        {
            AddValue = Math.Min(0, AddValue),
            AddBasePercent = Math.Min(0, AddBasePercent),
            MultiplyTotal = Math.Min(1, MultiplyTotal),
            Minimum = Minimum,
            Maximum = Maximum,
        };
    }

    public override string ToString()
    {
        return $"Additive: {AddValue}," +
               $" BasePercent: {AddBasePercent}," +
               $" MultiplyTotal: {MultiplyTotal}," +
               $" Minimum: {Minimum}," +
               $" Maximum: {Maximum}";
    }

    /// <summary>
    /// Combines the modifiers from two separate stacks into one.
    /// </summary>
    /// <param name="left">The first mod to add.</param>
    /// <param name="right">The second mod to add to the first.</param>
    /// <returns>A new modifier with the combined values.</returns>
    [Pure]
    public static ModifierStack operator +(ModifierStack left, ModifierStack right)
    {
        left.AddValue += right.AddValue;
        left.AddBasePercent += right.AddBasePercent;
        left.MultiplyTotal *= right.MultiplyTotal;

        left.Minimum = Math.Max(left.Minimum, right.Minimum);
        left.Maximum = Math.Min(left.Maximum, right.Maximum);

        return left;
    }

    /// <summary>
    /// Multiplies a modifier stack by a scalar.
    /// </summary>
    /// <param name="mod">Modifier to scale.</param>
    /// <param name="scalar">Real value to scale by.</param>
    /// <returns>A scaled modifier result.</returns>
    [Pure]
    public static ModifierStack operator *(ModifierStack mod, int scalar)
    {
        mod.AddValue *= scalar;
        mod.AddBasePercent *= scalar;
        mod.MultiplyTotal = Math.Pow(mod.MultiplyTotal, scalar);

        // Completely clueless as to how I'd modify min/max by a scalar... 
        return mod;
    }

    /// <summary>
    /// Multiplies a modifier stack by a scalar.
    /// </summary>
    /// <param name="scalar">Real value to scale by.</param>
    /// <param name="mod">Modifier to scale.</param>
    /// <returns>A scaled modifier result.</returns>
    [Pure]
    public static ModifierStack operator *(int scalar, ModifierStack mod) => mod * scalar;
}

public enum ModifierOperation
{
    AddValue = 0,
    AddBasePercent = 1,
    MultiplyTotal = 2,
    Minimum = 3,
    Maximum = 4,
}