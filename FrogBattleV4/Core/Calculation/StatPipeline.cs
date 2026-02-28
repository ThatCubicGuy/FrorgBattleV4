using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Calculation;

public static class StatPipeline
{
    [Pure]
    public static double ComputeStat(this ModifierContext ctx, StatId stat)
    {
        if (ctx.Actor is { } actor)
        {
            return new StatQuery
            {
                Stat = stat
            }.Compute(actor.BaseStats.GetValueOrDefault(stat), ctx);
        }

        System.Diagnostics.Debug.WriteLine($"WARNING: Attempt to compute stat for null actor! (Stat: {stat})");
        return 0;
    }
}