using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public class StatusEffectInstance : ApplicableEffect
{
    public required StatusEffectDefinition Definition { get; init; }
    public int Turns { get; set; }
    public int Stacks { get; set; }

    [Pure]
    public override bool ShouldRemove() => Stacks <= 0 || (Turns <= 0 && !Definition.Properties.HasFlag(EffectFlags.Infinite));

    protected override int GetStacks(EntityUid subject, EntityUid? reference, BattleEnvironment env) => Stacks;
}