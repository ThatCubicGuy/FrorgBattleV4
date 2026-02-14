#nullable enable
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem.Pools;

namespace FrogBattleV4.Core.BattleSystem;

public interface IHasPools
{
    IReadOnlyDictionary<PoolId, PoolComponent> Pools { get; }
    
    bool AddPool(PoolComponent pool);
    bool RemovePool(PoolId id);
}

public static class IHasPoolsExtensions
{
    [Pure]
    public static PoolComponent? GetPoolById([NotNull] this IHasPools pools, [NotNull] PoolId poolId)
    {
        return pools.Pools.GetValueOrDefault(poolId);
    }
    [Pure]
    public static IEnumerable<PoolComponent> GetPoolsByTag([NotNull] this IHasPools pools, PoolTag tag)
    {
        return pools.Pools.Values.Where(pc => pc.HasTag(tag));
    }
}