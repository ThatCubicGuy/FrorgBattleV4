using System;
using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Modifiers;

[JsonConverter(typeof(NumberPredicateConverter))]
public class NumberPredicate : IGamePredicate<int>
{
    public NumberPredicate(int exact)
    {
        Min = exact;
        Max = exact;
    }

    public NumberPredicate(int? min, int? max)
    {
        Min = min;
        Max = max;
        if (!Min.HasValue && !Max.HasValue)
        {
            throw new ArgumentException("At least one number must be specified.");
        }

        if (Min > Max)
        {
            throw new ArgumentException("Min must be less than or equal to Max.");
        }
    }

    public int? Min { get; }
    public int? Max { get; }

    public bool Evaluate(int value)
    {
        return (!Min.HasValue || value >= Min.Value) && (!Max.HasValue || value <= Max.Value);
    }
}