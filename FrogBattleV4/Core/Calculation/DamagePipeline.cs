using System;
using FrogBattleV4.Core.Calculation.Damage;

namespace FrogBattleV4.Core.Calculation;

public static class DamagePipeline
{
    // Separate context input because DamageCommand supplies target and targeting.

    public static void ExecuteDamage(this DamageCommand cmd, ModifierContext ctx)
    {
        ctx = ctx with
        {
            Other = cmd.Target,
            Aiming = cmd.Targeting,
        };

        if (ctx.Rng is null) throw new ArgumentException("Damage context requires an RNG property.", nameof(ctx));

        var isCrit = cmd.CanCrit && ctx.Rng.NextDouble() < ctx.ComputeStat(StatId.CritRate);

        var finalAmount = new DamageQuery
        {
            Type = cmd.Type,
            Source = cmd.Source,
            Crit = isCrit,
        }.ComputeMut(cmd.BaseAmount * (isCrit ? 1 + ctx.ComputeStat(StatId.CritDamage) : 1), ctx);
        // Crit damage is applied directly to base damage ^

        // Def is applied after every calculation.
        finalAmount -= new ModifierContext
        {
            Actor = ctx.Other,
            Other = ctx.Actor,
            Rng = ctx.Rng,
        }.ComputeStat(StatId.Def);
        finalAmount = Math.Max(0, finalAmount);
        if (cmd.Target.Hitbox.Resolve(cmd.Targeting).WouldHit)
            cmd.Target.Pools.TakeDamage(new DamageResult(finalAmount, cmd.Target, cmd.Type, isCrit));
    }
}