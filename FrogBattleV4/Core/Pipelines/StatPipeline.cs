using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Pipelines;

public static class StatPipeline
{
    [Pure]
    public static double ComputeStat(this ModifierContext modCtx, StatId statId)
    {
        if (modCtx.Actor is null) return 0;
        return new StatQuery
        {
            Stat = statId
        }.Compute(modCtx.Actor.BaseStats.GetValueOrDefault(statId), modCtx);
    }
}