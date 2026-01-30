#nullable enable
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem;

public class Team
{
    // Interesting suggestion by predictive tests
    public readonly static List<Team> AllTeams = [];

    public Team(params IBattleMember[] battleMembers)
    {
        Members = battleMembers.ToList();
        AllTeams.Add(this);
    }

    public IReadOnlyList<IBattleMember> Members { get; }

    public IReadOnlyList<ITargetable> Parts => Members.SelectMany(m => m.Parts ?? []).ToList();
}