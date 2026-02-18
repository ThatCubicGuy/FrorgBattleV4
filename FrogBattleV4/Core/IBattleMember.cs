using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core;

public interface IBattleMember : ITakesTurns, DamageSystem.IDamageable, EffectSystem.ISupportsEffects, IHasAbilities
{
    [NotNull] string Name { get; }
    [NotNull] FrozenDictionary<StatId, double> BaseStats { get; }
    [NotNull] PoolContainer Pools { get; }
}

public static class BattleMemberExtensions
{
    /// <summary>
    /// Gets the team of which this BattleMember is a part of, or null if it is not part of them.
    /// </summary>
    /// <param name="bm">The member whose team to find.</param>
    /// <param name="teams">A list of teams to look through for the allied team.</param>
    /// <returns>The team which contains the given member.</returns>
    /// <exception cref="ArgumentNullException">member or list of teams is null.</exception>
    /// <exception cref="System.InvalidOperationException">The battle member is part of
    /// more than one of the given teams.</exception>
    [Pure]
    public static Team GetAlliedTeam([NotNull] this IBattleMember bm, [NotNull] IEnumerable<Team> teams)
    {
        ArgumentNullException.ThrowIfNull(bm);
        ArgumentNullException.ThrowIfNull(teams);
        return teams.SingleOrDefault(team => team.Members.Contains(bm));
    }

    [Pure]
    public static PoolComponent GetPoolById([NotNull] this IBattleMember bm, PoolId poolId)
    {
        ArgumentNullException.ThrowIfNull(bm);
        return bm.Pools[poolId];
    }

    [Pure]
    public static IEnumerable<PoolComponent> GetPoolsByTag([NotNull] this IBattleMember bm, PoolTag tag)
    {
        return bm.Pools.Where(pc => pc.HasTag(tag));
    }
}