#nullable enable
using System;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class DamageRes : IDamageModifier
{
    public string? Type { get; init; }
    public string? Source { get; init; }
    public required double Amount { get; init; }
    public ModifierOperation Operation { get; init; } = ModifierOperation.MultiplyTotal;

    public DamagePhase Phase => Type is null ? DamagePhase.RawRes : DamagePhase.TypeRes;

    public DamageCalcContext Apply(DamageCalcContext ctx)
    {
        if ((Type ?? ctx.Type) != ctx.Type ||
            (Source ?? ctx.Source) != ctx.Source) return ctx;
        
        ctx.Mods[Operation] = Operation switch
        {
            ModifierOperation.Maximum => Math.Min(Amount, ctx.Mods[Operation]),
            ModifierOperation.Minimum => Math.Max(Amount, ctx.Mods[Operation]),
            ModifierOperation.MultiplyTotal => ctx.Mods[Operation] * Amount,
            _ => ctx.Mods[Operation] + Amount
        };
        
        return ctx;
    }
}