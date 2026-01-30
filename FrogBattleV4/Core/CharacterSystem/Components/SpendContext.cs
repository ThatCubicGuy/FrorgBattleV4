namespace FrogBattleV4.Core.CharacterSystem.Components;

public struct SpendContext
{
    public required ICharacter Owner { get; init; }
    public AbilitySystem.AbilityDefinition Ability { get; init; }
    public SpendMode Mode { get; init; }
}

public enum SpendMode
{
    Preview,
    Validate,
    Commit
}