#nullable enable
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem.Pools;

namespace FrogBattleV4.Core.CharacterSystem;

public interface IHasStats
{
    IReadOnlyDictionary<string, double> BaseStats { get; }
    IReadOnlyDictionary<string, IPoolComponent> Pools { get; }
}

public static class IHasStatsExtensions
{
    [Pure]
    public static IPoolComponent? GetPoolById([NotNull] this IHasStats stats, [NotNull] string statId)
    {
        return stats.Pools.GetValueOrDefault(statId);
    }
    [Pure]
    public static IPoolComponent? GetPoolByFlag([NotNull] this IHasStats stats, PoolFlags flag)
    {
        return stats.Pools.Values.LastOrDefault(pc => pc.Flags.HasFlag(flag));
    }
}