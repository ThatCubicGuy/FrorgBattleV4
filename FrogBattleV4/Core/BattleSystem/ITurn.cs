namespace FrogBattleV4.Core.BattleSystem;

public interface ITurn
{
    IBattleMember Entity { get; }
    TurnContext TurnStatus { get; }
    double BaseActionValue { get; }
    bool StartTurn(BattleContext ctx);
    void EndTurn();
}