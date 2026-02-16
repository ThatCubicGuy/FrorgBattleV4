#nullable enable
using FrogBattleV4.Core.EffectSystem.Modifiers;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class StatModifier : ModifierRule<StatQuery>
{
    public required StatId Stat { get; init; }

    protected override bool AppliesToQuery(StatQuery query)
    {
        return query.Stat == Stat;
    }
}