#nullable enable
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem;

public interface IBattleMember
{
    IAction[]? Turns { get; }
    /// <summary>
    /// Usually just one member, but in theory bigger bosses could have multiple targetable parts.
    /// </summary>
    ITargetable[]? Parts { get; }
    
    double GetStat(string stat, IBattleMember? target = null);
}