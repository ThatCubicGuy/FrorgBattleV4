using System;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core;

/// <summary>
/// Represents a combined collection of modifiers for a certain value.
/// </summary>
/// <param name="AddValue">Adds this value to the total, first step.</param>
/// <param name="AddBasePercent">Adds this value multiplied by the base value, second step.</param>
/// <param name="MultiplyTotal">Multiplies the total by this value, third step.</param>
/// <param name="FinalAddValue">Adds this value to the total, fourth step.</param>
public record ModifierStack(
    double AddValue = 0,
    double AddBasePercent = 0,
    double MultiplyTotal = 1,
    double FinalAddValue = 0)
{
    public double this[ModifierOperation operation] => operation switch
    {
        ModifierOperation.AddValue => AddValue,
        ModifierOperation.AddBasePercent => AddBasePercent,
        ModifierOperation.MultiplyTotal => MultiplyTotal,
        ModifierOperation.FinalAddValue => FinalAddValue,
        _ => throw new ArgumentOutOfRangeException(nameof(operation), $"Invalid ModifierOperation! ({operation})")
    };

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
        total += this[ModifierOperation.FinalAddValue];
        return total;
    }

    /// <summary>
    /// Returns a copy of this ModifierStack that
    /// retains only the positive or neutral effects.
    /// </summary>
    /// <returns>A new ModifierStack.</returns>
    [Pure]
    public ModifierStack AsPositive() => new()
    {
        AddValue = Math.Max(0, AddValue),
        AddBasePercent = Math.Max(0, AddBasePercent),
        MultiplyTotal = Math.Max(1, MultiplyTotal),
        FinalAddValue = Math.Max(0, AddValue),
    };

    /// <summary>
    /// Returns a copy of this ModifierStack that
    /// retains only the negative or neutral effects.
    /// </summary>
    /// <returns>A new ModifierStack.</returns>
    [Pure]
    public ModifierStack AsNegative() => new()
    {
        AddValue = Math.Min(0, AddValue),
        AddBasePercent = Math.Min(0, AddBasePercent),
        MultiplyTotal = Math.Min(1, MultiplyTotal),
        FinalAddValue = Math.Min(0, AddValue),
    };

    public override string ToString()
    {
        return $"Additive: {AddValue}," +
               $" BasePercent: {AddBasePercent}," +
               $" MultiplyTotal: {MultiplyTotal}," +
               $" FinalAdditive: {FinalAddValue}";
    }

    /// <summary>
    /// Combines the modifiers from two separate stacks into one.
    /// </summary>
    /// <param name="left">The first mod to add.</param>
    /// <param name="right">The second mod to add to the first.</param>
    /// <returns>A new modifier with the combined values.</returns>
    [Pure]
    public static ModifierStack operator +(ModifierStack left, ModifierStack right) => new()
    {
        AddValue = left.AddValue + right.AddValue,
        AddBasePercent = left.AddBasePercent + right.AddBasePercent,
        MultiplyTotal = left.MultiplyTotal * right.MultiplyTotal,
        FinalAddValue = left.FinalAddValue + right.FinalAddValue,
    };

    /// <summary>
    /// Multiplies a modifier stack by a scalar.
    /// </summary>
    /// <param name="mod">Modifier to scale.</param>
    /// <param name="scalar">Integer to scale by.</param>
    /// <returns>A scaled modifier result.</returns>
    [Pure]
    public static ModifierStack operator *(ModifierStack mod, int scalar) => new()
    {
        AddValue = mod.AddValue * scalar,
        AddBasePercent = mod.AddBasePercent * scalar,
        MultiplyTotal = Math.Pow(mod.MultiplyTotal, scalar),
        FinalAddValue = mod.FinalAddValue * scalar,
    };

    /// <summary>
    /// Multiplies a modifier stack by a scalar.
    /// </summary>
    /// <param name="scalar">Integer to scale by.</param>
    /// <param name="mod">Modifier to scale.</param>
    /// <returns>A scaled modifier result.</returns>
    [Pure]
    public static ModifierStack operator *(int scalar, ModifierStack mod) => mod * scalar;
    
    /// <summary>
    /// Multiplies a modifier stack by a scalar.
    /// </summary>
    /// <param name="mod">Modifier to scale.</param>
    /// <param name="scalar">Real value to scale by.</param>
    /// <returns>A scaled modifier result.</returns>
    [Pure]
    public static ModifierStack operator *(ModifierStack mod, double scalar) => new()
    {
        AddValue = mod.AddValue * scalar,
        AddBasePercent = mod.AddBasePercent * scalar,
        MultiplyTotal = Math.Pow(mod.MultiplyTotal, scalar),
        FinalAddValue = mod.FinalAddValue * scalar,
    };

    /// <summary>
    /// Multiplies a modifier stack by a scalar.
    /// </summary>
    /// <param name="scalar">Real value to scale by.</param>
    /// <param name="mod">Modifier to scale.</param>
    /// <returns>A scaled modifier result.</returns>
    [Pure]
    public static ModifierStack operator *(double scalar, ModifierStack mod) => mod * scalar;
}

public enum ModifierOperation
{
    AddValue,
    AddBasePercent,
    MultiplyTotal,
    FinalAddValue,
}