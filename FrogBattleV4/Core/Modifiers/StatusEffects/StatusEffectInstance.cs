using System;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Combat.Actions;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public class StatusEffectInstance(ApplyEffect ctx) : ApplicableEffect
{
    public StatusEffectDefinition Definition { get; } = ctx.Data.Effect;
    public int Turns { get; set; } = ctx.Data.InitialTurns;
    public int Stacks { get; set; } = ctx.Data.AddedStacks;
    public EffectFlags Props { get; init; } = EffectFlags.None;

    [Pure]
    public bool ShouldRemove() => Stacks <= 0 || Turns <= 0;

    protected override int GetStacks(EntityUid subject, EntityUid? reference, BattleEnvironment env) => Stacks;
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