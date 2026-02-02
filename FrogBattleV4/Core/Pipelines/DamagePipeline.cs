#nullable enable
using System;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.CharacterSystem;

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
        rawDamage = (ctx.Attacker as Character)?.AggregateMods(ctx, ctx.Target).ApplyTo(rawDamage) ?? rawDamage;

        // Crit bonus
        if (ctx.IsCrit)
        {
            rawDamage *= 1 + (ctx.Attacker?.GetStat(nameof(Stat.CritDamage), ctx.Target) ?? 0);
        }

        // Incoming damage resistances
        rawDamage = (ctx.Target as Character)?.AggregateMods(ctx, ctx.Attacker).ApplyTo(rawDamage) ?? rawDamage;

        // DEF Application
        rawDamage -= (ctx.Target?.GetStat(nameof(Stat.Def), ctx.Attacker) ?? 0) * Math.Clamp(1 - ctx.DefPen, 0, 1);

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
    public static DamagePreview PreviewDamage(this DamageRequest req, DamageExecContext ctx)
    {
        var minDamage = new DamageCalcContext
        {
            Attacker = ctx.Source,
            Target = ctx.Target,
            IsCrit = false,
            Type = req.Properties.Type,
            Source = "attack"
        }.ComputePipeline(req.BaseAmount);

        var maxDamage = new DamageCalcContext
        {
            Attacker = ctx.Source,
            Target = ctx.Target,
            IsCrit = true,
            Type = req.Properties.Type,
            Source = "attack"
        }.ComputePipeline(req.BaseAmount);

        return new DamagePreview(req.Target, minDamage, maxDamage);
    }

    /// <summary>
    /// Calculates and applies an instance of damage to the target in the given context. 
    /// </summary>
    /// <param name="req">Request to process.</param>
    /// <param name="ctx">Context in which to process.</param>
    public static void ExecuteDamage(this DamageRequest req, DamageExecContext ctx)
    {
        // Resolve RNG
        var isCrit = req.CanCrit &&
                     ctx.Rng.NextDouble() < ctx.Source?.GetStat(nameof(Stat.CritRate), ctx.Target);

        // Send to the compute mines
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
}