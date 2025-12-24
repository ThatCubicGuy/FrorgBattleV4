#nullable enable
using System;
using System.Linq;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.Pipelines;

public static class DamagePipeline
{
    public static double ComputePipeline(this DamageCalcContext ctx)
    {
        var total = ctx.RawDamage;
        // Outgoing damage bonuses
        if (ctx.Attacker is { } attacker)
        {
            var outCtx = attacker.AttachedEffects
                .SelectMany(x => x.Modifiers)
                .OfType<IDamageModifier>()
                .Aggregate(ctx, (current, mod) =>
                    mod.Apply(current));
        
            // Apply outgoing mods
            total += outCtx.Mods[ModifierOperation.AddValue];
            total += outCtx.Mods[ModifierOperation.AddBasePercent] * ctx.RawDamage;
            total *= outCtx.Mods[ModifierOperation.MultiplyTotal];
            total = Math.Clamp(total,
                outCtx.Mods[ModifierOperation.Minimum],
                outCtx.Mods[ModifierOperation.Maximum]);
        
            // Crit bonus
            if (ctx.Properties.CanCrit && attacker.GetStat(nameof(Stat.CritRate), ctx.Target) < ctx.Rng.NextDouble())
            {
                total *= attacker.GetStat(nameof(Stat.CritDamage), ctx.Target);
            }
        }
        
        // Reset current damage context cuz yk. Phase switch. Stuff like that.
        ctx = ctx with { RawDamage = total, Mods = default };
        
        // Incoming damage resistances
        if (ctx.Target is { } target)
        {
            var inCtx = target.AttachedEffects
                .SelectMany(x => x.Modifiers)
                .OfType<IDamageModifier>()
                .Aggregate(ctx, (current, mod) =>
                    mod.Apply(current));

            // Apply incoming mods
            total += inCtx.Mods[ModifierOperation.AddValue];
            total += inCtx.Mods[ModifierOperation.AddBasePercent] * ctx.RawDamage;
            total *= inCtx.Mods[ModifierOperation.MultiplyTotal];
            total = Math.Clamp(total,
                inCtx.Mods[ModifierOperation.Minimum],
                inCtx.Mods[ModifierOperation.Maximum]);

            // DEF Application
            total -= target.GetStat(nameof(Stat.Def), ctx.Attacker) * Math.Clamp(1 - ctx.Properties.DefPen, 0, 1);
        }
        
        return Math.Max(0, total);
    }
}