using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.Calculation;

public static class MutationPipeline
{
    /// <summary>
    /// Previews the complete effects of a mutation.
    /// </summary>
    /// <param name="cmd">Mutation intent to preview.</param>
    /// <param name="ctx">Context in which to calculate mutation.</param>
    /// <returns>A mutation result previewing how the pool would mutate.</returns>
    [Pure]
    public static MutationResult PreviewMutation(this MutationCommand cmd, ModifierContext ctx)
    {
        var finalAmount = new PoolMutQuery
        {
            PoolId = cmd.TargetPool,
            Channel = cmd.BaseAmount > 0 ? PoolMutChannel.Regen : PoolMutChannel.Cost,
            Ctx = ctx
        }.ComputeMut(cmd.BaseAmount);
        return new MutationResult(cmd.TargetPool, finalAmount);
    }
}