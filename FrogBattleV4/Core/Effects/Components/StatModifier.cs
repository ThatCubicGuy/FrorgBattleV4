#nullable enable
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Effects.Modifiers;

namespace FrogBattleV4.Core.Effects.Components;

public class StatModifier : ModifierRule<StatQuery>
{
    public required StatId Stat { get; init; }

    protected override bool AppliesToQuery(StatQuery query)
    {
        return query.Stat == Stat;
    }
}