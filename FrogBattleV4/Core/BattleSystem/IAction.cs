namespace FrogBattleV4.Core.BattleSystem;

public interface IAction
{
    IBattleMember Entity { get; }
    bool CanExecute(BattleContext ctx);
    void Execute(BattleContext ctx);
}