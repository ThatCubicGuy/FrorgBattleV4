using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.EffectSystem;

public struct EffectContext
{
    public ICharacter Holder;
    public ICharacter Source;
    public StatusEffectDefinition Definition;
    public uint Turns;
    public uint Stacks;
}