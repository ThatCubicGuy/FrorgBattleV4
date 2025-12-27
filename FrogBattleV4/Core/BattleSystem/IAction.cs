#nullable enable
using FrogBattleV4.Core.BattleSystem.Decisions;

namespace FrogBattleV4.Core.BattleSystem;

public interface IAction
{
    IBattleMember Entity { get; }
    double BaseActionValue { get; }
    IDecisionRequester[]? Decisions { get; }
    bool CanTakeAction(BattleContext ctx);
    void StartAction(BattleContext ctx);
    void EndAction(BattleContext ctx);
}