using System.Diagnostics.Contracts;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.EffectSystem;

namespace FrogBattleV4.Core.Pipelines;

internal static class PoolPipeline
{
    /// <summary>
    /// Calculates different channel modifiers for a pool (such as cost or max).
    /// </summary>
    /// <param name="ctx">The context for which to run the calculations.</param>
    /// <param name="baseAmount">The base value of the cap/cost/regen.</param>
    /// <returns>The final value.</returns>
    [Pure]
    private static double ComputePipeline(this PoolValueCalcContext ctx, double baseAmount)
    {
        if (ctx.Actor is not ISupportsEffects owner) return baseAmount;
        var finalMods = owner.AggregateMods(ctx, ctx.Other);
        return finalMods.ApplyTo(baseAmount);
    }

    [Pure]
    public static MutationResult PreviewMutation(this MutationRequest req, MutationExecContext ctx)
    {
        var amount = req.BaseAmount;
        var pool = req.Selector(ctx);
        var total = new PoolValueCalcContext
        {
            Channel = req.BaseAmount > 0 ? PoolPropertyChannel.Regen : PoolPropertyChannel.Cost,
            Actor = ctx.Holder,
            Other = ctx.Other,
            PoolId = req.Selector(ctx).Id,
            Flags = req.Flags
        }.ComputePipeline(amount);

        return new MutationResult(pool, total,
            total <= pool.CurrentValue);
    }

    public static void ExecuteMutation(this MutationRequest req, MutationExecContext ctx)
    {
        var result = req.PreviewMutation(ctx);
        result.ResultTarget.CurrentValue += result.FinalDeltaValue;
    }
}