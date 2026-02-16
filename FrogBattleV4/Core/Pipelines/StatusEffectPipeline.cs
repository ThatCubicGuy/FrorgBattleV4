using System.ComponentModel;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.EffectSystem;

namespace FrogBattleV4.Core.Pipelines;

internal static class StatusEffectPipeline
{
    /// <summary>
    /// Calculates the total chance of applying an effect in this context.
    /// </summary>
    /// <param name="statusEffect">The status effect context to apply.</param>
    /// <returns>Final application chance value.</returns>
    /// <exception cref="InvalidEnumArgumentException">Chance type is invalid.</exception>
    [Pure]
    public static double ComputeTotalChance(this StatusEffectApplicationContext statusEffect)
    {
        var baseCtx = new ModifierContext(statusEffect.Source, statusEffect.Target as BattleMember);
        var revCtx = new ModifierContext(baseCtx.Other, baseCtx.Actor);
        return statusEffect.ChanceType switch
        {
            ChanceType.Fixed => statusEffect.ApplicationChance,
            ChanceType.Base => statusEffect.ApplicationChance +
                               baseCtx.ComputeStat(StatId.EffectHitRate) -
                               revCtx.ComputeStat(StatId.EffectRes),
            _ => throw new InvalidEnumArgumentException($"Invalid chance type: {statusEffect.ChanceType}")
        };
    }
}