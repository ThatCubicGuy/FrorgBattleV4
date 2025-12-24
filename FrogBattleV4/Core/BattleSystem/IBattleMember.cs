#nullable enable
namespace FrogBattleV4.Core.BattleSystem;

public interface IBattleMember
{
    ITurn[]? Turns { get; }
    IAction[]? Actions { get; }
}