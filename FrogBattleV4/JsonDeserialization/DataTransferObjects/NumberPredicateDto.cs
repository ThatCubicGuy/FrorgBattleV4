using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.JsonDeserialization.DataTransferObjects;

public class NumberPredicateDto
{
    public int? Min { get; init; }
    public int? Max { get; init; }

    public NumberPredicate ToNumberPredicate()
    {
        return new NumberPredicate(Min, Max);
    }
}