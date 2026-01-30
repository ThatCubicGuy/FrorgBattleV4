using System;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core;

// This doesn't quite make sense, but I had an idea lmao
public struct Modifiers()
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
            _ => throw new System.IndexOutOfRangeException($"Invalid ModifierOperation! ({index})")
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
                default: throw new System.IndexOutOfRangeException($"Invalid ModifierOperation! ({index})");
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
    public static Modifiers Add(this Modifiers modifiers, ModifierOperation operation, double baseAmount)
    {
        modifiers[operation] = operation switch
        {
            ModifierOperation.Maximum => Math.Min(baseAmount, modifiers[operation]),
            ModifierOperation.Minimum => Math.Max(baseAmount, modifiers[operation]),
            ModifierOperation.MultiplyTotal => modifiers[operation] * baseAmount,
            _ => modifiers[operation] + baseAmount
        };
        return modifiers;
    }
    [Pure]
    public static double Apply(this Modifiers modifiers, double baseAmount, double total)
    {
        total += modifiers[ModifierOperation.AddValue];
        total += modifiers[ModifierOperation.AddBasePercent] * baseAmount;
        total *= modifiers[ModifierOperation.MultiplyTotal];
        total = Math.Clamp(total,
            modifiers[ModifierOperation.Minimum],
            modifiers[ModifierOperation.Maximum]);
        return total;
    }
}