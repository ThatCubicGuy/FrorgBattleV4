using System.Collections.Generic;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem;

public class Team
{
    // Interesting suggestion by predictive tests
    public static List<Team> AllTeams;
    public IBattleMember[] Members = new IBattleMember[5];
    public Team EnemyTeam;
}