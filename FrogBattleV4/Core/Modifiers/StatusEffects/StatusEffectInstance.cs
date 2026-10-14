using System;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public class StatusEffectInstance : ApplicableEffect
{
    public required StatusEffectDefinition Definition { get; init; }
    public EffectFlags Properties { get; init; } = EffectFlags.None;
    public int Turns { get; set; }
    public int Stacks { get; set; }

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