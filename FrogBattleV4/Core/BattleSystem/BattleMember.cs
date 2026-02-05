#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.BattleSystem;

public abstract class BattleMember(string name) : IHasStats
{
    public string Name { get; } = name;
    [NotNull] public abstract IEnumerable<IAction> Turns { get; }

    /// <summary>
    /// Every individually selectable part that pertains to this battle member and thus shares its pools and stats.
    /// </summary>
    [NotNull] public abstract IEnumerable<IDamageable> Parts { get; }

    public abstract IReadOnlyDictionary<string, double> BaseStats { get; }
    public abstract IReadOnlyDictionary<string, IPoolComponent> Pools { get; }


    /// <summary>
    /// Calculates the final value of a stat, optionally in relation to an enemy.
    /// </summary>
    /// <param name="stat">The name of the stat to calculate.</param>
    /// <param name="target">The enemy against which to calculate the stat. Optional.</param>
    /// <returns>The final value of the stat.</returns>
    public double GetStat(string stat, BattleMember? target = null)
    {
        return new StatCalcContext
        {
            Stat = stat,
            Actor = this,
            Other = target
        }.ComputePipeline();
    }
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