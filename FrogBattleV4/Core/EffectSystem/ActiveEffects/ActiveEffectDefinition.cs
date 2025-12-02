#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.ActiveEffects;

public class ActiveEffectDefinition : IEffectDefinition
{
    public required string Id { get; init; }
    public required IReadOnlyList<IModifierComponent> Modifiers { get; init; }
    public required IReadOnlyList<IMutatorComponent> Mutators { get; init; }
    
    public uint MaxStacks { get; init; }
    public uint MaxDuration { get; init; }
}