#nullable enable
using System;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Modifiers;

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
    private static double ComputePipeline(this DamageCalcContext ctx, double rawDamage)
    {
        var query = new DamageQuery
        {
            Type = ctx.Type,
            Source = ctx.Source,
            Crit = ctx.IsCrit,
            Direction = ModifierDirection.Outgoing
        };

        // Outgoing damage bonuses from the attacker
        if (ctx.Attacker is ISupportsEffects actor)
        {
            var actorMods = query.AggregateMods(new EffectInfoContext
            {
                Holder = actor,
                Other = ctx.Other
            });
            rawDamage = actorMods.ApplyTo(rawDamage);
        }

        // Crit bonus
        if (ctx.IsCrit)
        {
            rawDamage *= 1 + (ctx.Attacker?.GetStat(StatId.CritDamage, ctx.Other) ?? 0);
        }

        // Incoming damage resistances from the target
        if (ctx.Other is ISupportsEffects other)
        {
            query = query with { Direction = ModifierDirection.Incoming };
            var otherMods = query.AggregateMods(new EffectInfoContext
            {
                Holder = other,
                Other = ctx.Attacker
            });
            rawDamage = otherMods.ApplyTo(rawDamage);
        }

        // DEF Application
        rawDamage -= (ctx.Other?.GetStat(StatId.Def, ctx.Attacker) ?? 0) * Math.Clamp(1 - ctx.DefPen, 0, 1);

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
        req.Properties.Deconstruct(out var type, out var defPen, out var typeResPen);
        var baseCtx = new DamageCalcContext
        {
            IsCrit = false,
            Attacker = ctx.Source,
            Other = ctx.Other,
            Type = type,
            Source = "attack",
            DefPen = defPen,
            TypeResPen = typeResPen,
        };

        var minDamage = baseCtx.ComputePipeline(req.BaseAmount);

        var maxDamage = req.CanCrit ? (baseCtx with
        {
            IsCrit = true
        }).ComputePipeline(req.BaseAmount) : minDamage;

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
                     ctx.Rng.NextDouble() < ctx.Source?.GetStat(StatId.CritRate, ctx.Other);

        // Send to the compute mines
        var damage = new DamageCalcContext
        {
            Attacker = ctx.Source,
            Other = ctx.Other,
            IsCrit = isCrit,
            Type = req.Properties.Type,
            Source = "attack"
        }.ComputePipeline(req.BaseAmount);

        var result = new DamageResult(req.Target, damage, req.Properties.Type, isCrit);

        // TODO

        
    }
}