using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Pipelines;

public static class StatPipeline
{
    [Pure]
    public static double ComputeStat(this ModifierContext modCtx, StatId statId)
    {
        if (modCtx.Actor is null) return 0;
        return modCtx.Resolve(new StatQuery { Stat = statId })
            .ApplyTo(modCtx.Actor.BaseStats.GetValueOrDefault(statId));
    }
}