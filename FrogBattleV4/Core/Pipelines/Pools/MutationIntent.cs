using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.Pipelines.Pools;

/// <summary>
/// Initializes a new mutation request with the given properties.
/// </summary>
/// <param name="Selector">Selector to extract a pool to mutate from the target.</param>
/// <param name="BaseAmount">Base amount of the mutation.</param>
/// <param name="Flags">Mutation flags, such as... immutability of the mutation.</param>
public record MutationIntent(
    double BaseAmount,
    [NotNull] PoolSelector Selector,
    PoolMutationFlags Flags = PoolMutationFlags.None);

[return: NotNull] public delegate PoolComponent PoolSelector(ModifierContext ctx);

public static class MutationSelector
{
    [Pure]
    public static PoolSelector ById(PoolId poolId)
    {
        return Selector;

        PoolComponent Selector(ModifierContext ctx)
        {
            return ctx.Actor?.GetPoolById(poolId) ??
                   throw new InvalidOperationException("Pool not found for " + poolId);
        }
    }

    [Pure]
    public static PoolSelector ByAllTags([NotNull] params PoolTag[] poolTags)
    {
        return Selector;

        PoolComponent Selector(ModifierContext ctx)
        {
            return ctx.Actor?.Pools.Values.LastOrDefault(pc => pc.HasAllTags(poolTags)) ??
                   throw new InvalidOperationException($"Pool not found for tags [{string.Join(", ", poolTags.Select(tag => $"\"{tag}\""))}]");
        }
    }

    /// <summary>
    /// Picks the first pool that satisfies the first tag, or the first pool that satisfies the second, etc.
    /// </summary>
    /// <param name="poolTags">Pool tags to scan in order.</param>
    /// <returns>A new MutationRequest.</returns>
    [Pure]
    public static PoolSelector ByAnyTags([NotNull] params PoolTag[] poolTags)
    {
        return Selector;

        PoolComponent Selector(ModifierContext ctx)
        {
            // This is probably a horrible way to do this. Too bad!
            var pool = poolTags.Aggregate((PoolComponent)null, (accumulate, flag) =>
                accumulate ?? ctx.Actor?.GetPoolsByTag(flag).LastOrDefault());
            return pool ?? throw new InvalidOperationException("Cannot find a pool for any of the flags given.");
        }
    }
}