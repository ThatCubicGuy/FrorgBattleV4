using System.Collections.Generic;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.Pipelines;
internal static class StatPipeline
{
    /// <summary>
    /// Calculates the total value of a stat.
    /// </summary>
    /// <param name="ctx">The context in which to run the calculations.</param>
    /// <returns>The final value.</returns>
    [Pure]
    public static double ComputePipeline(this StatCalcContext ctx)
    {
        if (ctx.Actor is not ISupportsEffects owner) return ctx.Actor.BaseStats.GetValueOrDefault(ctx.Stat, 0);
        var finalMods = owner.AggregateMods(ctx, ctx.Other);

        return finalMods.ApplyTo(ctx.Actor.BaseStats.GetValueOrDefault(ctx.Stat, 0));
    }
}