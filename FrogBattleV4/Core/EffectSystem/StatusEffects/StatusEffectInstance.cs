#nullable enable
using System;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.EffectSystem.StatusEffects;

public class StatusEffectInstance(StatusEffectApplicationContext ctx) : ApplicableEffect
{
    public StatusEffectDefinition Definition { get; } = ctx.Definition;
    public ISupportsEffects Holder { get; } = ctx.Target;
    public IBattleMember? EffectSource { get; } = ctx.Source;
    public int Turns { get; set; } = ctx.InitialTurns;
    public int Stacks { get; set; } = ctx.AddedStacks;
    public EffectFlags Props { get; init; } = EffectFlags.None;

    [Pure]
    public bool ShouldRemove() => Stacks <= 0 || Turns <= 0;

    protected override ModifierCollection ModifierCollection => Definition.Modifiers;
    protected override int GetStacks(ModifierContext ctx) => Stacks;
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