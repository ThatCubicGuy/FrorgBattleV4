#nullable enable

using System;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.Extensions;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class DamageRes : IDamageModifier
{
    public string? Type { get; init; }
    public string? Source { get; init; }
    public required double Amount { get; init; }
    public ModifierOperation Operation { get; init; } = ModifierOperation.MultiplyTotal;

    public DamagePhase Phase => Type is null ? DamagePhase.RawRes : DamagePhase.TypeRes;

    public double Apply(double currentValue, DamageContext ctx)
    {
        if ((Type ?? ctx.Properties.Type) != ctx.Properties.Type ||
            (Source ?? ctx.Source) != ctx.Source) return currentValue;
        
        return Operation.Apply(Amount, ctx.RawDamage, currentValue);
    }
}