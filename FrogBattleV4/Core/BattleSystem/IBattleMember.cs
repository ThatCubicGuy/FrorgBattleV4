#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem;

public interface IBattleMember
{
    string Name { get; }
    IAction[]? Turns { get; }
    /// <summary>
    /// Usually just one member, but in theory bigger bosses could have multiple targetable parts.
    /// </summary>
    ITargetable[]? Parts { get; }
    
    double GetStat(string stat, IBattleMember? target = null);
}

public static class IBattleMemberExtensions
{
    /// <summary>
    /// Gets the team of which this IBattleMember is a part of, or null if it is not part of them.
    /// </summary>
    /// <param name="member"></param>
    /// <param name="teams"></param>
    /// <returns>The team which contains </returns>
    /// <exception cref="System.InvalidOperationException">The battle member is part of more than one of the teams.</exception>
    public static Team? GetAlliedTeam(this IBattleMember member, IEnumerable<Team>? teams)
    {
        return teams?.SingleOrDefault(x => x.Members.Contains(member));
    }
}