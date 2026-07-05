using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;

namespace FrogBattleV4.Core.Entities;

public interface IBattleMember
{
    /// <summary>
    /// The name of this battle member.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Team that this BattleMember is a part of.
    /// </summary>
    Team AlliedTeam { get; }

    double GetStat(StatQuery query);
}