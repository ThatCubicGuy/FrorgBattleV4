using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Pipelines.Pools;

namespace FrogBattleV4.Core.Pipelines;

public static class PoolPipeline
{
    /// <summary>
    /// Previews the complete effects of a mutation.
    /// </summary>
    /// <param name="mut">Mutation intent to preview.</param>
    /// <param name="ctx">Context in which to calculate mutation.</param>
    /// <returns>A mutation result previewing how the pool would mutate.</returns>
    [Pure]
    public static MutationResult PreviewMutation(this MutationIntent mut, ModifierContext ctx)
    {
        var pool = mut.Selector(ctx);
        var finalAmount = new PoolMutQuery
        {
            PoolId = pool.Id,
            Channel = mut.BaseAmount < 0 ? PoolMutChannel.Cost : PoolMutChannel.Regen,
        }.ComputeMut(mut.BaseAmount, ctx);
        return new MutationResult(pool, finalAmount, finalAmount <= pool.CurrentValue);
    }

    public static void ExecuteMutation(this MutationIntent req, ModifierContext ctx)
    {
        var result = req.PreviewMutation(ctx);
        if (result.Allowed) result.ResultTarget.CurrentValue -= result.FinalDeltaValue;
    }
}