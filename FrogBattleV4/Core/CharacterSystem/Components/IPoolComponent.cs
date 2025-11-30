namespace FrogBattleV4.Core.CharacterSystem.Components;

public interface IPoolComponent
{
    ICharacter Owner { get; init; }
    string Id { get; }
    double CurrentValue { get; }
}