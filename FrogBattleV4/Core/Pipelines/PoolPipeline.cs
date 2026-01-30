using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem;

namespace FrogBattleV4.Core.Pipelines;

public static class PoolPipeline
{
    /// <summary>
    /// Calculates different channel modifiers for a pool (such as cost or max).
    /// </summary>
    /// <param name="ctx">The context for which to run the calculations.</param>
    /// <param name="baseAmount">The base value of the cap/cost/regen.</param>
    /// <returns>The final value.</returns>
    public static double ComputePipeline(this PoolCalcContext ctx, double baseAmount)
    {
        var finalMods = ctx.Owner.AttachedEffects
            .AggregateMods(ctx, new EffectContext
            {
                Holder = ctx.Owner,
                Target = null,
            });
        
        return finalMods.Apply(baseAmount);
    }
}