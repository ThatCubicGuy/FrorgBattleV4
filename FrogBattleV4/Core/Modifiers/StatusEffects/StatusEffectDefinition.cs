using System.Collections.Generic;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public class StatusEffectDefinition
{
    public required string Id { get; init; }
    public ModifierRuleCollection ModifierRules { get; init; } = new();
    public IEnumerable<IMutatorComponent> Mutators { get; init; } = [];

    public required uint MaxStacks { get; init; }
    public required uint MaxDuration { get; init; }
}