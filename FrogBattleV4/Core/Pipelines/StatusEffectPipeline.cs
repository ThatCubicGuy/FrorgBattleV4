using System.ComponentModel;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem;
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
        return statusEffect.ChanceType switch
        {
            ChanceType.Fixed => statusEffect.ApplicationChance,
            ChanceType.Base => statusEffect.ApplicationChance +
                               (statusEffect.Source.GetStat(nameof(Stat.EffectHitRate), statusEffect.Target as BattleMember)) -
                               ((statusEffect.Target as BattleMember)?.GetStat(nameof(Stat.EffectRes), statusEffect.Source) ?? 0),
            _ => throw new InvalidEnumArgumentException($"Invalid chance type: {statusEffect.ChanceType}")
        };
    }
}