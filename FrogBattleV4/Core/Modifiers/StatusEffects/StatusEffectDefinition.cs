using System;
using System.Collections.Immutable;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public class StatusEffectDefinition
{
    public required StatusEffectId Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public ImmutableArray<IModifierRule> ModifierRules { get; init; } = [];
    public ImmutableArray<IMutatorComponent> Mutators { get; init; } = [];
    public EffectFlags Properties { get; init; } = EffectFlags.None;

    public required int MaxStacks { get; init; }
    public int MaxDuration { get; init; }
}

[Flags]
public enum EffectFlags
{
    None = 0,
    Unremovable = 1 << 0,
    Invisible = 1 << 1,
    Infinite = 1 << 2,
    StartTick = 1 << 3,
    RemoveStack = 1 << 4,
}