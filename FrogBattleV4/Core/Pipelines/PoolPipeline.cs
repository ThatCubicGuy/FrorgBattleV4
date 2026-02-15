using System.Diagnostics.Contracts;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Modifiers;

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
    public static double ComputePipeline(this PoolValueCalcContext ctx, double baseAmount)
    {
        var mods = new ModifierStack();
        if (ctx.Holder is ISupportsEffects owner)
        {
            mods += new PoolQuery
            {
                PoolId = ctx.PoolId,
                Channel = ctx.Channel,
                Direction = ModifierDirection.Incoming,
            }.AggregateMods(new EffectInfoContext
            {
                Holder = owner,
                Other = ctx.Other,
            });
        }

        if (ctx.Other is ISupportsEffects other)
        {
            mods += new PoolQuery
            {
                PoolId = ctx.PoolId,
                Channel = ctx.Channel,
                Direction = ModifierDirection.Outgoing,
            }.AggregateMods(new EffectInfoContext
            {
                Holder = other,
                Other = ctx.Holder as BattleMember,
            });
        }

        return mods.ApplyTo(baseAmount);
    }

    [Pure]
    public static MutationResult PreviewMutation(this MutationRequest req, MutationExecContext ctx)
    {
        var amount = req.BaseAmount;
        var pool = req.Selector(ctx);
        var total = new PoolValueCalcContext
        {
            Channel = req.BaseAmount > 0 ? PoolPropertyChannel.Regen : PoolPropertyChannel.Cost,
            Holder = ctx.Holder,
            Other = ctx.Other,
            PoolId = req.Selector(ctx).Id,
            Flags = req.Flags,
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