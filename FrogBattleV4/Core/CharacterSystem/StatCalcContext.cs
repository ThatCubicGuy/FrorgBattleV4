#nullable enable

namespace FrogBattleV4.Core.CharacterSystem;

public struct StatCalcContext()
{
    public string Stat;
    public IHasStats Owner;
    public IHasStats? Target;
    public Modifiers Mods = default;
}