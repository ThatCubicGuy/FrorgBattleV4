using FrogBattleV4.Core.CharacterSystem.Components;

namespace FrogBattleV4.Core.CharacterSystem;

public struct PoolCalcContext()
{
    public string PoolId;
    public ICharacter Owner;
    public PoolModType Channel;
    public Modifiers Mods = default;
}