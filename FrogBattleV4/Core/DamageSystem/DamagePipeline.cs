using System;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.DamageSystem;

public static class DamagePipeline
{
    // I don't like this. I just threw something together to feel better about myself.
    // fricking chatgpt i gotta stop using it less if i wanna actually get anywhere
    public static double ComputePipeline(DamageContext ctx)
    {
        var mods = ctx.Attacker.ActiveEffects.SelectMany(x => x.Modifiers)
            .OfType<DamageBonus>().Sum(x => x.Amount);
        var total = ctx.RawDamage;
        total *= 1 + mods;
        total *= ctx.Properties.CanCrit &&
                 ctx.Rng.NextDouble() < ctx.Attacker.GetStat(nameof(Stat.CritRate), ctx.Target)
            ? 1 + ctx.Attacker.GetStat(nameof(Stat.CritDamage), ctx.Target)
            : 1;
        total -= ctx.Target.GetStat(nameof(Stat.Def), ctx.Attacker) * Math.Clamp(1 - ctx.Properties.DefPen, 0, 1);
        mods = ctx.Target.ActiveEffects.SelectMany(x => x.Modifiers)
            .OfType<DamageRes>().Sum(x => x.Amount);
        total *= Math.Clamp(1 - mods * Math.Clamp(1 - ctx.Properties.TypeResPen, 0, 1), 0, 1);
        return total;
    }
}