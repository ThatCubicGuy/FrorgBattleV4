#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.ActiveEffects;

public class ActiveEffectDefinition : IAttributeModifier
{
    public required List<IModifierComponent> Modifiers { get; init; }
    
    public required List<IMutatorComponent> Mutators { get; init; }
    
    public uint MaxStacks { get; init; }
    public uint MaxDuration { get; init; }
}