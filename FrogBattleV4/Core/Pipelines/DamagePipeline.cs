using System.Diagnostics.Contracts;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.Pipelines;

public static class DamagePipeline
{
    // Separate context input because DamageIntent supplies target and targeting.
    [Pure]
    public static DamagePreview PreviewDamage(this DamageIntent dmg, DamageExecContext ctx)
    {
        var modCtx = new ModifierContext
        {
            Actor = ctx.Source,
            Other = dmg.Target as BattleMember,
            Ability = ctx.Definition,
            Aiming = dmg.Targeting,
            Rng = ctx.Rng,
        };
        var normalDamage = modCtx.Resolve(new DamageQuery
        {
            Type = dmg.Type,
            Source = DamageSource.Ability,
            Crit = false,
        }).ApplyTo(dmg.BaseAmount);
        
        var critDamage = modCtx.Resolve(new DamageQuery
        {
            Type = dmg.Type,
            Source = DamageSource.Ability,
            Crit = true,
        }).ApplyTo(dmg.BaseAmount);

        return new DamagePreview(dmg.Target, normalDamage, critDamage);
    }

    public static DamageResult ExecuteDamage(this DamageIntent dmg, DamageExecContext ctx)
    {
        var modCtx = new ModifierContext
        {
            Actor = ctx.Source,
            Other = dmg.Target as BattleMember,
            Ability = ctx.Definition,
            Aiming = dmg.Targeting,
            Rng = ctx.Rng,
        };
        var isCrit = ctx.Rng.NextDouble() < modCtx.ComputeStat(StatId.CritRate);
        var finalAmount = modCtx.Resolve(new DamageQuery
        {
            Type = dmg.Type,
            Source = DamageSource.Ability,
            Crit = isCrit,
        }).ApplyTo(dmg.BaseAmount);
        return new DamageResult(finalAmount, dmg.Target, dmg.Type, isCrit);
    }
}