#nullable enable
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.DamageSystem;

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

    public IReadOnlyCollection<BattleMember> Members { get; }

    public IReadOnlyCollection<IDamageable> Parts => Members.SelectMany(m => m.Parts ?? []).ToList();
}