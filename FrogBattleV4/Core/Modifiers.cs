namespace FrogBattleV4.Core;

// This doesn't quite make sense, but I had an idea lmao
public readonly struct Modifiers()
{
    private readonly double[] _modifierValues = [0, 0, 1, double.NaN, double.NaN];
    public double this[ModifierOperation operation]
    {
        get => _modifierValues[(int)operation];
        set => _modifierValues[(int)operation] = value;
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