#nullable enable
using System.Collections.Generic;

namespace FrogBattleV4.Core.CharacterSystem;

public struct StatContext()
{
    public string Stat;
    public IHasStats Owner;
    public IHasStats? Target;
    public double[] ModifierValues = [0, 0, 0, double.NaN, double.NaN];
}