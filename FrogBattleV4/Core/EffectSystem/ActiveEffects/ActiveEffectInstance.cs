#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.ActiveEffects;

public class ActiveEffectInstance : IAttributeModifier
{
    public required ActiveEffectDefinition Definition { get; init; }
    public ICharacter? Source { get; init; }
    public uint Turns { get; set; }
    public uint Stacks { get; set; }

    IReadOnlyList<IModifierComponent> IAttributeModifier.Modifiers => Definition.Modifiers;
}