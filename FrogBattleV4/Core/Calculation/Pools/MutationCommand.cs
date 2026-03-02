using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;

namespace FrogBattleV4.Core.Calculation.Pools;

/// <summary>
/// Initializes a new mutation request with the given properties.
/// </summary>
/// <param name="PoolSelector">Selector to extract a pool to mutate from the target.</param>
/// <param name="BaseAmount">Base amount of the mutation.</param>
/// <param name="Flags">Mutation flags, such as... immutability of the mutation.</param>
public record MutationCommand(
    double BaseAmount,
    [NotNull] Func<ModifierContext, PoolComponent> PoolSelector,
    PoolMutationFlags Flags = PoolMutationFlags.None) : IBattleCommand;

[Flags] public enum PoolMutationFlags
{
    None = 0,
    Immutable = 1 << 0
}

public static class MutationSelector
{
    /// <summary>
    /// Finds a pool by its ID, or throws an exception if the pool was not found.
    /// </summary>
    /// <param name="poolId">PoolID to search for.</param>
    /// <returns>A selector function that returns a pool with the given ID.</returns>
    /// <exception cref="InvalidOperationException">Pool was not found.</exception>
    [Pure]
    public static Func<ModifierContext, PoolComponent> ById(PoolId poolId)
    {
        return Selector;

        PoolComponent Selector(ModifierContext ctx)
        {
            return ctx.Actor?.Pools[poolId] ??
                   throw new InvalidOperationException("Pool not found for " + poolId);
        }
    }

    /// <summary>
    /// Finds the last pool that has the given tag, or throws an exception if no pool has the requested tag.
    /// </summary>
    /// <param name="poolTag">Pool tag to search for.</param>
    /// <returns>A selector function that returns the last pool with the given tag.</returns>
    /// <exception cref="InvalidOperationException">No pool has the given tag.</exception>
    [Pure]
    public static Func<ModifierContext, PoolComponent> LastByTag(PoolTag poolTag)
    {
        return Selector;

        PoolComponent Selector(ModifierContext ctx)
        {
            return ctx.Actor?.Pools.WithTag(poolTag).LastOrDefault() ??
                   throw new InvalidOperationException("Pool not found for " + poolTag);
        }
    }
}