using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Pipelines.Pools;

namespace FrogBattleV4.Core.Pipelines;

public static class PoolPipeline
{
    [Pure]
    public static MutationResult PreviewMutation(this MutationIntent mut, ModifierContext ctx)
    {
        var pool = mut.Selector(ctx);
        var finalAmount = ctx.Resolve(new PoolValueQuery
        {
            PoolId = pool.Id,
            Channel = mut.BaseAmount < 0 ? PoolPropertyChannel.Cost : PoolPropertyChannel.Regen,
        }).ApplyTo(mut.BaseAmount);
        return new MutationResult(pool, finalAmount, finalAmount <= pool.CurrentValue);
    }

    public static void ExecuteMutation(this MutationIntent req, ModifierContext ctx)
    {
        var result = req.PreviewMutation(ctx);
        if (result.Allowed) result.ResultTarget.CurrentValue -= result.FinalDeltaValue;
    }
}