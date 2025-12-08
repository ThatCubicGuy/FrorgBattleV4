using System;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.DamageSystem;

public static class DamagePipeline
{
    public static double ComputePipeline(this DamageContext ctx)
    {
        // Outgoing damage bonuses
        var outMods = ctx.Attacker.AttachedEffects.SelectMany(x => x.Modifiers
            .OfType<IDamageModifier>()
            .Where(y => (y.Type ?? ctx.Type) == ctx.Type))
            .ToArray();
        
        // Raw damage bonuses (universal Damage Boost)
        var total = outMods.Where(x => x.Phase == DamagePhase.RawBonus)
            .Aggregate(ctx.RawDamage, (current, modifier) => modifier.Apply(current, ctx));
        
        // Crit bonus
        if (ctx.Properties.CanCrit && ctx.Attacker.GetStat(nameof(Stat.CritRate), ctx.Target) < ctx.Rng.NextDouble())
        {
            total *= ctx.Attacker.GetStat(nameof(Stat.CritDamage), ctx.Target);
        }
        
        // Post crit bonuses (Type Bonus)
        total = outMods.Where(x => x.Phase  == DamagePhase.TypeBonus)
            .Aggregate(total, (current, modifier) => modifier.Apply(current, ctx));
        
        // Incoming damage resistances
        var inMods = ctx.Target.AttachedEffects.SelectMany(x => x.Modifiers
                .OfType<IDamageModifier>()
                .Where(y => (y.Type ?? ctx.Type) == ctx.Type))
            .ToArray();

        // Pre DEF bonuses (Type Res)
        total = inMods.Where(x => x.Phase == DamagePhase.TypeRes)
            .Aggregate(total, (current, modifier) =>
                modifier.Apply(current, ctx) * Math.Clamp(1 - ctx.Properties.TypeResPen, 0, 1));
        
        // DEF Application
        total -= ctx.Target.GetStat(nameof(Stat.Def), ctx.Attacker) * Math.Clamp(1 - ctx.Properties.DefPen, 0, 1);
        
        // Final touches (universal DMG Res)
        total = inMods.Where(x => x.Phase == DamagePhase.RawRes)
            .Aggregate(total, (current, modifier) => modifier.Apply(current, ctx));
        
        return total;
    }
}