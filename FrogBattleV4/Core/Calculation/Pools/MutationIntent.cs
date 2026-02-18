using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace FrogBattleV4.Core.Calculation.Pools;

/// <summary>
/// Initializes a new mutation request with the given properties.
/// </summary>
/// <param name="PoolSelector">Selector to extract a pool to mutate from the target.</param>
/// <param name="BaseAmount">Base amount of the mutation.</param>
/// <param name="Flags">Mutation flags, such as... immutability of the mutation.</param>
public record MutationIntent(
    double BaseAmount,
    [NotNull] Func<ModifierContext, PoolComponent> PoolSelector,
    PoolMutationFlags Flags = PoolMutationFlags.None);

[Flags] public enum PoolMutationFlags
{
    None = 0,
    Immutable = 1 << 0
}

public static class MutationSelector
{
    [Pure]
    public static Func<ModifierContext, PoolComponent> ById(PoolId poolId)
    {
        return Selector;

        PoolComponent Selector(ModifierContext ctx)
        {
            return ctx.Actor?.GetPoolById(poolId) ??
                   throw new InvalidOperationException("Pool not found for " + poolId);
        }
    }

    [Pure]
    public static Func<ModifierContext, PoolComponent> ByAllTags([NotNull] params PoolTag[] poolTags)
    {
        return Selector;

        PoolComponent Selector(ModifierContext ctx)
        {
            return ctx.Actor?.Pools.LastOrDefault(pc => pc.HasAllTags(poolTags)) ??
                   throw new InvalidOperationException($"Pool not found for tags [{string.Join(", ", poolTags.Select(tag => $"\"{tag}\""))}]");
        }
    }

    /// <summary>
    /// Picks the first pool that satisfies the first tag, or the first pool that satisfies the second, etc.
    /// </summary>
    /// <param name="poolTags">Pool tags to scan in order.</param>
    /// <returns>A new MutationRequest.</returns>
    [Pure]
    public static Func<ModifierContext, PoolComponent> ByAnyTags([NotNull] params PoolTag[] poolTags)
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