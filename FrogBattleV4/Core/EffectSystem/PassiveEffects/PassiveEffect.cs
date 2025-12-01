#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects;

public class PassiveEffect : IEffectDefinition, IAttributeModifier
{
    public required string Id { get; init; }
    public required List<IModifierComponent> Modifiers { get; init; }
    public required List<IConditionComponent> Conditions { get; init; }

    public double GetModifiedStat(string statName, double currentValue, EffectContext ctx)
    {
        throw new System.NotImplementedException();
    }
}