#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.BattleSystem;

public class Team
{
    // Interesting suggestion by predictive tests
    public readonly static List<Team> AllTeams = [];

    public Team(params BattleMember[] battleMembers)
    {
        Members = battleMembers.ToList();
        AllTeams.Add(this);
    }

    public IEnumerable<BattleMember> Members { get; }
}