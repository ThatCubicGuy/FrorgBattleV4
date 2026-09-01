using System;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Abilities.Components.Actions;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public class StatusEffectInstance(ApplyEffectAction ctx) : ApplicableEffect
{
    public StatusEffectDefinition Definition { get; } = ctx.Definition;
    public int Turns { get; set; } = ctx.InitialTurns;
    public int Stacks { get; set; } = ctx.AddedStacks;
    public EffectFlags Props { get; init; } = EffectFlags.None;

    [Pure]
    public bool ShouldRemove() => Stacks <= 0 || Turns <= 0;

    protected override ModifierRuleCollection ModifierRuleCollection => Definition.ModifierRules;
    protected override int GetStacks(RelationContext ctx) => Stacks;
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