using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.BattleSystem;

public class Team : IEnumerable<IBattleMember>
{
    // Interesting suggestion by predictive tests
    public static List<Team> AllTeams;
    public List<IBattleMember> Members { get; } = new List<IBattleMember>(4);
    public Team EnemyTeam { get; init; }
    public IBattleMember this[int index] => Members.ElementAt(index);
    public IEnumerator<IBattleMember> GetEnumerator() => Members.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}