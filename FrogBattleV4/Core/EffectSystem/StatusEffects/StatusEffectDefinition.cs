using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.EffectSystem.StatusEffects;

public class StatusEffectDefinition
{
    [NotNull] public required string Id { get; init; }
    [NotNull] public ModifierCollection Modifiers { get; init; } = new();
    [NotNull] public IEnumerable<IMutatorComponent> Mutators { get; init; } = [];

    public required uint MaxStacks { get; init; }
    public required uint MaxDuration { get; init; }
}