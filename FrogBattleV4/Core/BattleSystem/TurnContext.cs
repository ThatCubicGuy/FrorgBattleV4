using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem;

public struct TurnContext
{
    public ICharacter ActiveCharacter { get; init; }
    public uint TurnNumber { get; init; }
}