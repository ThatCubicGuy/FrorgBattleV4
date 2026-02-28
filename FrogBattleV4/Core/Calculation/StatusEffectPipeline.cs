using System.ComponentModel;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Effects;

namespace FrogBattleV4.Core.Calculation;

internal static class StatusEffectPipeline
{
    /// <summary>
    /// Calculates the total chance of applying an effect in this context.
    /// </summary>
    /// <param name="cmd">The status effect command to apply.</param>
    /// <returns>Final application chance value.</returns>
    /// <exception cref="InvalidEnumArgumentException">Chance type is invalid.</exception>
    [Pure]
    public static double ComputeTotalChance(this ApplyEffectCommand cmd)
    {
        var outCtx = new ModifierContext(cmd.EffectSource, cmd.Target);
        var inCtx = new ModifierContext(cmd.Target, cmd.EffectSource);
        return cmd.ChanceType switch
        {
            ChanceType.Fixed => cmd.ApplicationChance,
            ChanceType.Base => cmd.ApplicationChance +
                               new StatQuery(StatId.EffectHitRate).Compute(
                                   cmd.EffectSource.BaseStats[StatId.EffectHitRate]
                                   + cmd.ApplicationChance, outCtx) -
                               inCtx.ComputeStat(StatId.EffectRes),
            _ => throw new InvalidEnumArgumentException($"Invalid chance type: {cmd.ChanceType}")
        };
    }
}