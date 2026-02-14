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
    /// <param name="baseStatValue">Base stat value to consider.</param>
    /// <returns>The final value.</returns>
    [Pure]
    public static double ComputePipeline(this StatCalcContext ctx, double baseStatValue)
    {
        // Mods on the holder
        baseStatValue = new StatQuery
        {
            Stat = ctx.Stat,
            Channel = StatChannel.Owned,
        }.AggregateMods(new EffectInfoContext
        {
            Holder = ctx.Actor,
            Other = ctx.Other
        }).ApplyTo(baseStatValue);

        if (ctx.Other is not ISupportsEffects other)
            return baseStatValue;

        // Penalty mods on the other
        baseStatValue = new StatQuery
        {
            Stat = ctx.Stat,
            Channel = StatChannel.Penalty,
        }.AggregateMods(new EffectInfoContext
        {
            Holder = other,
            Other = ctx.Actor
        }).ApplyTo(baseStatValue);

        return baseStatValue;
    }
}