using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.EffectSystem.ActiveEffects;

public struct ActiveEffectContext
{
    public ISupportsEffects Holder;
    public ICharacter Source;
    public ActiveEffectDefinition Definition;
    public uint Turns;
    public uint Stacks;
}