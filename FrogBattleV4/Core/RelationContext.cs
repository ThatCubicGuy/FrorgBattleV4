namespace FrogBattleV4.Core;

public readonly record struct RelationContext
{
    public required EntityUid Actor { get; init; }
    public required EntityUid Target { get; init; }
}