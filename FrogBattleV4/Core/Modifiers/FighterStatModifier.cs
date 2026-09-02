using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

public class FighterStatModifier : StatModifier<StatQuery>
{
    public required StatId Stat { get; init; }

    protected override bool AppliesTo(StatQuery query)
    {
        return query.Stat == Stat;
    }
}