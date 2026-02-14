namespace FrogBattleV4.Core.CharacterSystem;

public struct StatQuery()
{
    public required StatId Stat { get; init; }
    public StatChannel Channel { get; init; } = StatChannel.Owned;
}

public enum StatChannel
{
    Owned,
    Penalty
}