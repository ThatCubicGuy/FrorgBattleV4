using System;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.Calculation;

public static class DamagePipeline
{
    // Separate context input because DamageIntent supplies target and targeting.
    [Pure]
    public static DamagePreview PreviewDamage(this DamageIntent dmg, DamageExecContext ctx)
    {
        var modCtx = new ModifierContext
        {
            Actor = ctx.Source,
            Other = dmg.Target as IBattleMember,
            Ability = ctx.Definition,
            Aiming = dmg.Targeting,
            Rng = ctx.Rng,
        };

        var normalDamage = new DamageQuery
        {
            Type = dmg.Type,
            Source = DamageSource.Ability,
            Crit = false,
        }.ComputeMut(dmg.BaseAmount, modCtx);

        var critDamage = new DamageQuery
        {
            Type = dmg.Type,
            Source = DamageSource.Ability,
            Crit = true,
        }.ComputeMut(dmg.BaseAmount, modCtx);

        return new DamagePreview(dmg.Target, normalDamage, critDamage);
    }

    public static void ExecuteDamage(this DamageIntent dmg, DamageExecContext ctx)
    {
        var modCtx = new ModifierContext
        {
            Actor = ctx.Source,
            Other = dmg.Target as IBattleMember,
            Ability = ctx.Definition,
            Aiming = dmg.Targeting,
            Rng = ctx.Rng,
        };

        var isCrit = dmg.CanCrit && ctx.Rng.NextDouble() < modCtx.ComputeStat(StatId.CritRate);

        var finalAmount = new DamageQuery
        {
            Type = dmg.Type,
            Source = DamageSource.Ability,
            Crit = isCrit,
        }.ComputeMut(dmg.BaseAmount * (isCrit ? 1 + modCtx.ComputeStat(StatId.CritDamage) : 1), modCtx);
        // Crit damage is applied directly to base damage ^

        // Def is applied after every calculation.
        finalAmount -= new ModifierContext
        {
            Other = modCtx.Actor,
            Actor = modCtx.Other,
            Rng = modCtx.Rng,
        }.ComputeStat(StatId.Def);
        finalAmount = Math.Max(0, finalAmount);
        if (dmg.Target.Hitbox.Resolve(dmg.Targeting).WouldHit)
            dmg.Target.TakeDamage(new DamageResult(finalAmount, dmg.Target, dmg.Type, isCrit));
    }
}