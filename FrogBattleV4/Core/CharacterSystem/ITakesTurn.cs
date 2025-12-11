using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.CharacterSystem;

public interface ITakesTurn
{
    bool InTurn { get; }
    double BaseActionValue { get; }
    bool TryEnterTurn(TurnContext ctx);
    void EnterTurn(TurnContext ctx);
}