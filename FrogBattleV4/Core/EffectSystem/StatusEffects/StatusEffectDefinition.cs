#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.EffectSystem.StatusEffects;

public class StatusEffectDefinition : IEffectDefinition
{
    public required string Id { get; init; }
    public IEnumerable<IModifierRule> Modifiers { get; init; } = [];
    public IEnumerable<IMutatorComponent> Mutators { get; init; } = [];

    public required uint MaxStacks { get; init; }
    public required uint MaxDuration { get; init; }
}