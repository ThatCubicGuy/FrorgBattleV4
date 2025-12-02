#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects;

public class PassiveEffect : IEffectDefinition, IAttributeModifier
{
    public required string Id { get; init; }
    public required IReadOnlyList<IModifierComponent> Modifiers { get; init; }
    public required IReadOnlyList<IConditionComponent> Conditions { get; init; }
    public double GetModifiedAttribute(string attribute, StatContext ctx)
    {
        throw new System.NotImplementedException();
    }
}