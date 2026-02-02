#nullable enable
using System.Threading.Tasks;
using FrogBattleV4.Core.BattleSystem.Decisions;

namespace FrogBattleV4.Core.BattleSystem;

public interface IAction
{
    BattleMember Entity { get; }
    double BaseActionValue { get; }
    Task PlayTurn(IDecisionProvider provider, BattleContext ctx);
    bool CanTakeAction(BattleContext ctx);
    void StartAction(BattleContext ctx);
    void EndAction(BattleContext ctx);
}