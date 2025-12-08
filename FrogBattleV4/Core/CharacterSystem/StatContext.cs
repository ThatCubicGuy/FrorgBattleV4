#nullable enable
namespace FrogBattleV4.Core.CharacterSystem;

public struct StatContext
{
    public string Stat;
    public ICharacter Owner;
    public ICharacter? Target;
}