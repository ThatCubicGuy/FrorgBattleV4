#nullable enable
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;

namespace FrogBattleV4.Core.CharacterSystem;

public interface IHasPools : IBattleMember
{
    IReadOnlyDictionary<string, IPoolComponent> Pools { get; }
}

public static class IHasPoolsExtensions
{
    [Pure]
    public static IPoolComponent? GetPoolById([NotNull] this IHasPools pools, [NotNull] string statId)
    {
        return pools.Pools.GetValueOrDefault(statId);
    }
    [Pure]
    public static IEnumerable<IPoolComponent> GetPoolsByFlag([NotNull] this IHasPools pools, PoolFlags flag)
    {
        return pools.Pools.Values.Where(pc => pc.Flags.HasFlag(flag));
    }
}