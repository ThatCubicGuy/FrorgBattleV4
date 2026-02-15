#nullable enable
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class StatModifier : ModifierRule<StatQuery>
{
    public required StatId Stat { get; init; }

    protected override bool AppliesToRequest(StatQuery query)
    {
        return query.Stat == Stat;
    }
}