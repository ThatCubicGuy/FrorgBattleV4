using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem;

public struct TurnContext
{
    public uint TurnNumber { get; init; }
    public TurnMoment Moment { get; init; }
}

public enum TurnMoment
{
    None = 0,
    Start,
    SelectAbility,
    SelectTarget,
    Animation,
    End
}