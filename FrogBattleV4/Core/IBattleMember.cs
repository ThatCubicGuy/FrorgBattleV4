using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;

namespace FrogBattleV4.Core;

public interface IBattleMember : ITakesTurns
{
    [NotNull] string Name { get; }
    [NotNull] ComponentContainer Components { get; }

    #region Caches

    AbilityContainer Abilities { get; }
    EffectContainer Effects { get; }
    StatContainer BaseStats { get; }
    PoolContainer Pools { get; }
    ITargetable Hitbox { get; }

    #endregion
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
}