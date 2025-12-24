using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.CharacterSystem;

public interface ITakesAction
{
    bool CanTakeAction(BattleContext ctx);
    void TakeAction(BattleContext ctx);
}