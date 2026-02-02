#nullable enable
using System;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.CharacterSystem;
using Microsoft.VisualBasic;

namespace FrogBattleV4.Core.Pipelines;

internal static class DamagePipeline
{
    /// <summary>
    /// Computes the full pipeline for dealing damage.
    /// </summary>
    /// <param name="ctx">The context in which to calculate damage.</param>
    /// <param name="rawDamage">The raw damage being dealt.</param>
    /// <returns>The final damage to be dealt.</returns>
    [Pure]
    private static double ComputePipeline(this DamageCalcContext ctx, double rawDamage)
    {
        // Outgoing damage bonuses
        // rawDamage = (ctx.Attacker as Character)?.AggregateMods(ctx, ctx.Target).ApplyTo(rawDamage) ?? rawDamage;
        if (ctx.Attacker is Character attacker)
        {
            var outMods = attacker.AggregateMods(ctx, ctx.Target);
            
            // Apply outgoing mods
            rawDamage = outMods.ApplyTo(rawDamage);

            // Crit bonus
            if (ctx.IsCrit)
            {
                rawDamage *= 1 + attacker.GetStat(nameof(Stat.CritDamage), ctx.Target);
            }
        }
        
        // Crit bonus
        if (ctx.IsCrit && ctx.Attacker is not null)
        {
            rawDamage *= 1 + ctx.Attacker.GetStat(nameof(Stat.CritDamage), ctx.Target);
        }
        // Incoming damage resistances
        // rawDamage = (ctx.Target as Character)?.AggregateMods(ctx, ctx.Attacker).ApplyTo(rawDamage) ?? rawDamage;
        if (ctx.Target is Character target)
        {
            var inMods = target.AggregateMods(ctx, ctx.Attacker);
            
            // Apply incoming mods
            rawDamage = inMods.ApplyTo(rawDamage);

            // DEF Application
            rawDamage -= target.GetStat(nameof(Stat.Def), ctx.Attacker) * Math.Clamp(1 - ctx.DefPen, 0, 1);
        }
        
        return Math.Max(0, rawDamage);
    }

    /// <summary>
    /// Returns a preview of the damage that is going to be dealt.
    /// This damage is going to then be piped through the IDamageable's ReceiveDamage() method, which
    /// might influence the real damage taken still. I simply cannot enforce fully accurate previews.
    /// </summary>
    /// <param name="req">Damage request to be processed.</param>
    /// <param name="ctx">Context in which to execute the request.</param>
    /// <returns>A preview of damage to be dealt.</returns>
    [Pure]
    public static DamageResult PreviewDamage(this DamageRequest req, DamageExecContext ctx)
    {
        var minDamage = new DamageCalcContext
        {
            Attacker = ctx.Source,
            Target = ctx.Target,
            IsCrit = false,
            Type = req.Properties.Type,
            Source = "attack"
        }.ComputePipeline(req.BaseAmount);
        
        // return new DamageResult(req.Target, damage, req.Properties.Type, isCrit);
    }

    public static void ExecuteDamage(this DamageRequest req, DamageExecContext ctx)
    {
        var isCrit = req.CanCrit &&
                     ctx.Rng.NextDouble() < ctx.Source?.GetStat(nameof(Stat.CritRate), ctx.Target);
        
        var damage = new DamageCalcContext
        {
            Attacker = ctx.Source,
            Target = ctx.Target,
            IsCrit = isCrit,
            Type = req.Properties.Type,
            Source = "attack"
        }.ComputePipeline(req.BaseAmount);
        
        var result = new DamageResult(req.Target, damage, req.Properties.Type, isCrit);
        
        req.Target.ReceiveDamage(result);
    }

    /// <summary>
    /// Metadata for computed calculation result.
    /// Needed to pass some info between Compute and Execute functions.
    /// Can also be used for display.
    /// </summary>
    /// <param name="Amount">Final damage amount.</param>
    /// <param name="IsCrit">Was it a critical hit?</param>
    private record DamageSnapshot(double Amount, string Type, bool IsCrit);
}