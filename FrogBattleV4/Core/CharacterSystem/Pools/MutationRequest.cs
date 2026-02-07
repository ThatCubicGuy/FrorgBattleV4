using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace FrogBattleV4.Core.CharacterSystem.Pools;

/// <summary>
/// Initializes a new mutation request with the given properties.
/// </summary>
/// <param name="Selector">Selector to extract a pool to mutate from the target.</param>
/// <param name="BaseAmount">Base amount of the mutation.</param>
/// <param name="Flags">Mutation flags, such as... immutability of the mutation.</param>
public record MutationRequest(
    [NotNull] PoolSelector Selector,
    double BaseAmount,
    PoolMutationFlags Flags = PoolMutationFlags.None);

[return: NotNull] public delegate IPoolComponent PoolSelector(MutationExecContext ctx);

public static class MutationRequestBuilder
{
    [Pure]
    public static MutationRequest ById(string poolId, double baseAmount,
        PoolMutationFlags flags = PoolMutationFlags.None)
    {
        return new MutationRequest(Selector, baseAmount, flags);

        IPoolComponent Selector(MutationExecContext ctx)
        {
            return ctx.Holder.GetPoolById(poolId) ??
                   throw new System.InvalidOperationException("Pool not found for " + poolId);
        }
    }

    [Pure]
    public static MutationRequest ByFlags(PoolFlags poolFlags, double baseAmount,
        PoolMutationFlags flags = PoolMutationFlags.None)
    {
        return new MutationRequest(Selector, baseAmount, flags);

        IPoolComponent Selector(MutationExecContext ctx)
        {
            return ctx.Holder.GetPoolsByFlag(poolFlags).LastOrDefault() ??
                   throw new System.InvalidOperationException("Pool not found for flags " + poolFlags);
        }
    }

    /// <summary>
    /// Picks the first pool that satisfies the first flag, or the first pool that satisfies the second, etc.
    /// </summary>
    /// <param name="baseAmount">Base amount of the mutation.</param>
    /// <param name="flags">Flags of the mutation.</param>
    /// <param name="poolFlagsList">Pool flags to scan in order.</param>
    /// <returns>A new MutationRequest.</returns>
    [Pure]
    public static MutationRequest ByAnyFlags([NotNull] IEnumerable<PoolFlags> poolFlagsList, double baseAmount,
        PoolMutationFlags flags = PoolMutationFlags.None)
    {
        var poolFlagsArray = poolFlagsList.ToArray();
        return new MutationRequest(Selector, baseAmount, flags);

        IPoolComponent Selector(MutationExecContext ctx)
        {
            var pool = poolFlagsArray.Aggregate((IPoolComponent)null, (accumulate, flag) =>
                accumulate ?? ctx.Holder.GetPoolsByFlag(flag).LastOrDefault());
            return pool ?? throw new System.InvalidOperationException("Cannot find a pool for any of the flags given.");
        }
    }

    [Pure]
    public static MutationRequest ByAnyFlags(double baseAmount, PoolMutationFlags flags,
        [NotNull] params PoolFlags[] poolFlagsArray)
    {
        return ByAnyFlags(poolFlagsArray, baseAmount, flags);
    }
}