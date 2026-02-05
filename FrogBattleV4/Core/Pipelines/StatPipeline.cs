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
        var result = ctx.Actor.BaseStats.GetValueOrDefault(ctx.Stat, 0);

        if (ctx.Actor is not ISupportsEffects owner)
            return result;

        // Mods on the holder
        var query = new StatQuery { Stat = ctx.Stat };
        var inMods = query.AggregateMods(new EffectInfoContext
        {
            Holder = owner,
            Other = ctx.Other
        });
        result = inMods.ApplyTo(result);

        if (ctx.Other is not ISupportsEffects other)
            return result;

        // Mods on the other
        query = query with { Channel = StatChannel.Penalty };
        var outMods = query.AggregateMods(new EffectInfoContext
        {
            Holder = other,
            Other = ctx.Actor
        });
        result = outMods.ApplyTo(result);

        return result;
    }
}