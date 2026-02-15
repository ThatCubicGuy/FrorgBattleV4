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
        var mods = new StatQuery
        {
            Stat = ctx.Stat,
            Channel = StatChannel.Owned,
        }.AggregateMods(new EffectInfoContext
        {
            Holder = ctx.Actor,
            Other = ctx.Other
        });

        if (ctx.Other is not ISupportsEffects other)
            return mods.ApplyTo(baseStatValue);

        // Penalty mods on the other
        mods += new StatQuery
        {
            Stat = ctx.Stat,
            Channel = StatChannel.Penalty,
        }.AggregateMods(new EffectInfoContext
        {
            Holder = other,
            Other = ctx.Actor
        });

        return mods.ApplyTo(baseStatValue);
    }
}