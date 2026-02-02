#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.BattleSystem;

public abstract class BattleMember(string name) : IHasStats
{
    public string Name { get; } = name;
    public abstract IAction[]? Turns { get; }

    /// <summary>
    /// Every individually selectable part that pertains to this battle member and thus shares its pools and stats.
    /// </summary>
    public abstract IDamageable[]? Parts { get; }

    public abstract IReadOnlyDictionary<string, double> BaseStats { get; }
    public abstract IReadOnlyDictionary<string, IPoolComponent> Pools { get; }

    public abstract double GetStat(string stat, BattleMember? target = null);
}

public static class BattleMemberExtensions
{
    /// <summary>
    /// Gets the team of which this BattleMember is a part of, or null if it is not part of them.
    /// </summary>
    /// <param name="member">The member whose team to find.</param>
    /// <param name="teams">A list of teams to look through for the allied team.</param>
    /// <returns>The team which contains the given member.</returns>
    /// <exception cref="ArgumentNullException">member or list of teams is null.</exception>
    /// <exception cref="System.InvalidOperationException">The battle member is part of
    /// more than one of the given teams.</exception>
    public static Team? GetAlliedTeam(this BattleMember member, IEnumerable<Team> teams)
    {
        ArgumentNullException.ThrowIfNull(member);
        ArgumentNullException.ThrowIfNull(teams);
        return teams.SingleOrDefault(x => x.Members.Contains(member));
    }
}