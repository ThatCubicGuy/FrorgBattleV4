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
        if (ctx.Actor is not ISupportsEffects owner)
            return baseStatValue;

        // Mods on the holder
        var query = new StatQuery { Stat = ctx.Stat };
        var inMods = query.AggregateMods(new EffectInfoContext
        {
            Holder = owner,
            Other = ctx.Other
        });
        baseStatValue = inMods.ApplyTo(baseStatValue);

        if (ctx.Other is not ISupportsEffects other)
            return baseStatValue;

        // Mods on the other
        query = query with { Channel = StatChannel.Penalty };
        var outMods = query.AggregateMods(new EffectInfoContext
        {
            Holder = other,
            Other = ctx.Actor
        });
        baseStatValue = outMods.ApplyTo(baseStatValue);

        return baseStatValue;
    }
}