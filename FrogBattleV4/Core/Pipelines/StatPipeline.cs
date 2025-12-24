using System;
using System.Linq;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Components;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.Pipelines;
public static class StatPipeline
{
    public static double ComputePipeline(this StatCalcContext ctx)
    {
        if (ctx.Owner is not ISupportsEffects owner) return ctx.Owner.BaseStats[ctx.Stat];
        var finalCtx = owner.AttachedEffects
            .SelectMany(x => x.Modifiers)
            .OfType<IStatModifier>()
            .Aggregate(ctx, (current, mod) =>
                mod.Apply(current));
        if (!ctx.Owner.BaseStats.TryGetValue(ctx.Stat, out var finalAmount)) return 0;
        finalAmount += finalCtx.Mods[ModifierOperation.AddValue];
        finalAmount += finalCtx.Mods[ModifierOperation.AddBasePercent] * ctx.Owner.BaseStats[ctx.Stat];
        finalAmount *= finalCtx.Mods[ModifierOperation.MultiplyTotal];
        finalAmount = Math.Clamp(finalAmount,
            finalCtx.Mods[ModifierOperation.Minimum],
            finalCtx.Mods[ModifierOperation.Maximum]);
        return finalAmount;
    }
}