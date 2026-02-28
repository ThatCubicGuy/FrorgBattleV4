namespace FrogBattleV4.Core.Combat;

public struct TurnContext
{
    public uint TurnNumber;
    public TurnMoment Moment;
}

public enum TurnMoment
{
    None = 0,
    Start,
    Selection,
    Animation,
    End
}