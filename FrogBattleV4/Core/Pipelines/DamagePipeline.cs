#nullable enable
using System;
using System.Linq;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.Pipelines;

public static class DamagePipeline
{
    public static double ComputePipeline(this DamageCalcContext ctx, double rawDamage)
    {
        var total = rawDamage;
        // Outgoing damage bonuses
        if (ctx.Attacker is { } attacker)
        {
            var outCtx = attacker.AttachedEffects
                .SelectMany(x => x.Modifiers)
                .OfType<IDamageModifier>()
                .Aggregate(ctx, (current, mod) =>
                    mod.Apply(current));
        
            // Apply outgoing mods
            total = outCtx.Mods.Apply(rawDamage, total);
        
            // Crit bonus
            if (ctx.Properties.CanCrit && attacker.GetStat(nameof(Stat.CritRate), ctx.Target) < ctx.Rng.NextDouble())
            {
                total *= attacker.GetStat(nameof(Stat.CritDamage), ctx.Target);
            }
        }
        
        // Reset current damage context cuz yk. Phase switch. Stuff like that.
        ctx.Mods = default;
        rawDamage = total;
        
        // Incoming damage resistances
        if (ctx.Target is { } target)
        {
            var inCtx = target.AttachedEffects
                .SelectMany(x => x.Modifiers)
                .OfType<IDamageModifier>()
                .Aggregate(ctx, (current, mod) =>
                    mod.Apply(current));

            // Apply incoming mods
            total = inCtx.Mods.Apply(rawDamage, total);

            // DEF Application
            total -= target.GetStat(nameof(Stat.Def), ctx.Attacker) * Math.Clamp(1 - ctx.Properties.DefPen, 0, 1);
        }
        
        return Math.Max(0, total);
    }
}