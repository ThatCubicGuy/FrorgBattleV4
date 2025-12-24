#nullable enable

namespace FrogBattleV4.Core.CharacterSystem;

public struct StatCalcContext()
{
    public string Stat { get; init; }
    public IHasStats Owner { get; init; }
    public IHasStats? Target { get; init; }
    public Modifiers Mods = default;
}