namespace FrogBattleV4.Core.Entities;

public abstract class FighterDefinition
{
    public required string Name { get; init; }
    public required FighterId Id { get; init; }
    // TODO (Nova): Sprite, abilities, documentation?
}

/// <summary>
/// Internal fighter name generated from JSON filename.
/// </summary>
public readonly record struct FighterId(string Id)
{
    public override string ToString() => Id;
}