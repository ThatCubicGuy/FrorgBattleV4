using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;

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

[return: NotNull] public delegate PoolComponent PoolSelector(MutationExecContext ctx);

public static class MutationRequestBuilder
{
    [Pure]
    public static MutationRequest ById(string poolId, double baseAmount,
        PoolMutationFlags flags = PoolMutationFlags.None)
    {
        return new MutationRequest(Selector, baseAmount, flags);

        PoolComponent Selector(MutationExecContext ctx)
        {
            return ctx.Holder.GetPoolById(poolId) ??
                   throw new InvalidOperationException("Pool not found for " + poolId);
        }
    }

    [Pure]
    public static MutationRequest ByTags(PoolTag[] poolTags, double baseAmount,
        PoolMutationFlags flags = PoolMutationFlags.None)
    {
        return new MutationRequest(Selector, baseAmount, flags);

        PoolComponent Selector(MutationExecContext ctx)
        {
            return ctx.Holder.Pools.Values.LastOrDefault(pc => pc.HasAllTags(poolTags)) ??
                   throw new InvalidOperationException($"Pool not found for tags [{string.Join(", ", poolTags.Select(tag => $"\"{tag}\""))}]");
        }
    }

    /// <summary>
    /// Picks the first pool that satisfies the first tag, or the first pool that satisfies the second, etc.
    /// </summary>
    /// <param name="baseAmount">Base amount of the mutation.</param>
    /// <param name="flags">Tags of the mutation.</param>
    /// <param name="poolTagsList">Pool tags to scan in order.</param>
    /// <returns>A new MutationRequest.</returns>
    [Pure]
    public static MutationRequest ByAnyFlags([NotNull] IEnumerable<PoolTag> poolTagsList, double baseAmount,
        PoolMutationFlags flags = PoolMutationFlags.None)
    {
        var poolFlagsArray = poolTagsList.ToArray();
        return new MutationRequest(Selector, baseAmount, flags);

        PoolComponent Selector(MutationExecContext ctx)
        {
            var pool = poolFlagsArray.Aggregate((PoolComponent)null, (accumulate, flag) =>
                accumulate ?? ctx.Holder.GetPoolsByTag(flag).LastOrDefault());
            return pool ?? throw new InvalidOperationException("Cannot find a pool for any of the flags given.");
        }
    }
}