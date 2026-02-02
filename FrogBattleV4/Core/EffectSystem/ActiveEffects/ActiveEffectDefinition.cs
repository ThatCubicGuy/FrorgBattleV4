#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.EffectSystem.ActiveEffects;

public class ActiveEffectDefinition : IEffectDefinition
{
    public required string Id { get; init; }
    public IReadOnlyList<IModifierComponent>? Modifiers { get; init; }
    public IReadOnlyList<IMutatorComponent>? Mutators { get; init; }
    
    public required uint MaxStacks { get; init; }
    public required uint MaxDuration { get; init; }
}