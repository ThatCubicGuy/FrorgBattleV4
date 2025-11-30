#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects;

public class PassiveEffect : IAttributeModifier
{
    public required List<IModifierComponent> Modifiers { get; init; }

    public required List<IConditionComponent> Conditions { get; init; }
}