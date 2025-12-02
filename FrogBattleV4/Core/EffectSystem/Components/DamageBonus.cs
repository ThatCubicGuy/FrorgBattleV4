#nullable enable

using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class DamageBonus : IDamageModifier
{
    public string? Type { get; init; }
    public string? Source { get; init; }
    public required double Amount { get; init; }

    public bool CanApply(DamagePhase phase, DamageContext ctx)
    {
        return phase == DamagePhase.PreMitigation && (Type == null || Type == ctx.Properties.DamageType) && (Source == null || Source == ctx.DamageSource);
    }
}