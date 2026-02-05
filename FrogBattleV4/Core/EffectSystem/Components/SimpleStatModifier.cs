#nullable enable
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class SimpleStatModifier : IModifierRule<StatQuery>
{
    public required ModifierStack ModifierStack { get; init; } = new();
    public required string Stat { get; init; }
    public StatChannel Channel { get; init; } = StatChannel.Owned;

    public bool AppliesFor(StatQuery query)
    {
        return query.Stat == Stat && query.Channel == Channel;
    }
}