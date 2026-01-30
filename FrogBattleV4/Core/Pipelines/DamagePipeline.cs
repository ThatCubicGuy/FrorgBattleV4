#nullable enable
using System;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.Pipelines;

public static class DamagePipeline
{
    /// <summary>
    /// Computes the full pipeline for dealing damage.
    /// </summary>
    /// <param name="ctx">The context in which to calculate damage.</param>
    /// <param name="rawDamage">The raw damage being dealt.</param>
    /// <returns>The final damage to be dealt.</returns>
    [Pure]
    public static double ComputePipeline(this DamageCalcContext ctx, double rawDamage)
    {
        // Outgoing damage bonuses
        if (ctx.Attacker is { } attacker)
        {
            var outMods = attacker.AttachedEffects
                .AggregateMods(ctx, new EffectContext
                {
                    Holder = attacker,
                    Target = ctx.Target
                });
            
            // Apply outgoing mods
            rawDamage = outMods.Apply(rawDamage);
            
            // Crit bonus
            if (ctx.Properties.CanCrit && attacker.GetStat(nameof(Stat.CritRate), ctx.Target) < ctx.Rng.NextDouble())
            {
                rawDamage *= attacker.GetStat(nameof(Stat.CritDamage), ctx.Target);
            }
        }
        
        // Incoming damage resistances
        if (ctx.Target is { } target)
        {
            var inMods = target.AttachedEffects
                .AggregateMods(ctx, new EffectContext
                {
                    Holder = target,
                    Target = ctx.Attacker
                });

            // Apply incoming mods
            rawDamage = inMods.Apply(rawDamage);

            // DEF Application
            rawDamage -= target.GetStat(nameof(Stat.Def), ctx.Attacker) * Math.Clamp(1 - ctx.Properties.DefPen, 0, 1);
        }
        
        return Math.Max(0, rawDamage);
    }
}