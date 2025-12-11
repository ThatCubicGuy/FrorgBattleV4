using System;
using System.Linq;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Components;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.Pipelines;
public static class StatPipeline
{
    public static double ComputePipeline(this StatContext ctx)
    {
        if (ctx.Owner is not ISupportsEffects owner) return ctx.Owner.BaseStats[ctx.Stat];
        var finalCtx = owner.AttachedEffects
            .SelectMany(x => x.Modifiers)
            .OfType<IStatModifier>()
            .Aggregate(ctx, (current, mod) =>
                mod.Apply(current));
        var finalAmount = ctx.Owner.BaseStats[ctx.Stat];
        finalAmount += finalCtx.ModifierValues[(int)ModifierOperation.AddValue];
        finalAmount += finalCtx.ModifierValues[(int)ModifierOperation.AddBasePercent] * ctx.Owner.BaseStats[ctx.Stat];
        finalAmount *= finalCtx.ModifierValues[(int)ModifierOperation.MultiplyTotal];
        return Math.Clamp(finalAmount,
            finalCtx.ModifierValues[(int)ModifierOperation.Minimum],
            finalCtx.ModifierValues[(int)ModifierOperation.Maximum]);
    }
}