#nullable enable
using System;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class DamageBonus : IDamageModifier
{
    public string? Type { get; init; }
    public string? Source { get; init; }
    public required double Amount { get; init; }
    public ModifierOperation Operation { get; init; } = ModifierOperation.MultiplyTotal;
    
    public DamagePhase Phase => Type is null ? DamagePhase.RawBonus : DamagePhase.TypeBonus;

    public DamageCalcContext Apply(DamageCalcContext ctx)
    {
        if ((Type ?? ctx.Type) != ctx.Type ||
            (Source ?? ctx.Source) != ctx.Source) return ctx;
        
        ctx.Mods = ctx.Mods.Add(Operation, Amount);
        
        return ctx;
    }
}