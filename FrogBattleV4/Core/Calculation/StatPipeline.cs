using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Calculation;

public static class StatPipeline
{
    [Pure]
    public static double ComputeStat(this ModifierContext ctx, StatId stat)
    {
        if (ctx.Actor is FighterBase actor)
        {
            return new StatQuery
            {
                Stat = stat,
                Ctx = ctx
            }.Compute(actor.BaseStats[stat]);
        }

        System.Diagnostics.Debug.WriteLine($"WARNING: Attempt to compute stat for null actor! (Stat: {stat})");
        return 0;
    }
}