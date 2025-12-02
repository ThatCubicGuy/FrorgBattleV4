#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.PassiveEffects;

public class PassiveEffect : IEffectDefinition, IAttributeModifier
{
    public required string Id { get; init; }
    public required List<IModifierComponent> Modifiers { get; init; }
    public required List<IConditionComponent> Conditions { get; init; }
    // AA I FUCKED IT UP CUZ NOW THERE'S NO WRAPPER AND CONDITIONS ARE IGNORED WHAT IS WRONG WITH ME
}