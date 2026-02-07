#nullable enable
using System.Threading.Tasks;
using FrogBattleV4.Core.BattleSystem.Selections;

namespace FrogBattleV4.Core.BattleSystem;

public interface IAction
{
    IBattleMember Entity { get; }
    double BaseActionValue { get; }
    Task PlayTurn(ISelectionProvider provider, BattleContext ctx);
    bool CanTakeAction(BattleContext ctx);
    void StartAction(BattleContext ctx);
    void EndAction(BattleContext ctx);
}