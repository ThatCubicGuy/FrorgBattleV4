using System;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Calculation.Pools;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.Calculation;

public static class DamagePipeline
{
    // Separate context input because DamageIntent supplies target and targeting.
    [Pure]
    public static DamagePreview PreviewDamage(this DamageCommand dmg, ModifierContext ctx)
    {
        if (ctx.Rng is null) throw new ArgumentException("Damage context requires an RNG property.", nameof(ctx));
        ctx = ctx with
        {
            Other = dmg.Target,
            Aiming = dmg.Targeting,
        };

        var normalDamage = new DamageQuery
        {
            Type = dmg.Type,
            Source = DamageSource.Ability,
            Crit = false,
        }.ComputeMut(dmg.BaseAmount, ctx);

        var critDamage = new DamageQuery
        {
            Type = dmg.Type,
            Source = DamageSource.Ability,
            Crit = true,
        }.ComputeMut(dmg.BaseAmount, ctx);

        return new DamagePreview(dmg.Target, normalDamage, critDamage);
    }

    public static void ExecuteDamage(this DamageCommand dmg, ModifierContext ctx)
    {
        ctx = ctx with
        {
            Other = dmg.Target,
            Aiming = dmg.Targeting,
        };

        if (ctx.Rng is null) throw new ArgumentException("Damage context requires an RNG property.", nameof(ctx));

        var isCrit = dmg.CanCrit && ctx.Rng.NextDouble() < ctx.ComputeStat(StatId.CritRate);

        var finalAmount = new DamageQuery
        {
            Type = dmg.Type,
            Source = DamageSource.Ability,
            Crit = isCrit,
        }.ComputeMut(dmg.BaseAmount * (isCrit ? 1 + ctx.ComputeStat(StatId.CritDamage) : 1), ctx);
        // Crit damage is applied directly to base damage ^

        // Def is applied after every calculation.
        finalAmount -= new ModifierContext
        {
            Actor = ctx.Other,
            Other = ctx.Actor,
            Rng = ctx.Rng,
        }.ComputeStat(StatId.Def);
        finalAmount = Math.Max(0, finalAmount);
        if (dmg.Target.Hitbox.Resolve(dmg.Targeting).WouldHit)
            dmg.Target.TakeDamage(new DamageResult(finalAmount, dmg.Target, dmg.Type, isCrit));
    }

    private static void TakeDamage(this IBattleMember bm, DamageResult dmg)
    {
        var pool = bm.Pools.WithTag(PoolTag.AbsorbsDamage).LastOrDefault() ??
                   bm.Pools.WithTag(PoolTag.UsedForLife).LastOrDefault();
        if (pool is null) return;
        pool.CurrentValue -= dmg.Amount;
    }
}