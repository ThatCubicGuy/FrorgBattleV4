using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Calculation;

public static class StatPipeline
{
    [Pure]
    public static double ComputeStat(this ModifierContext ctx, StatId stat)
    {
        if (ctx.Actor is null) return 0;
        return new StatQuery
        {
            Stat = stat
        }.Compute(ctx.Actor.BaseStats.GetValueOrDefault(stat), ctx);
    }
}