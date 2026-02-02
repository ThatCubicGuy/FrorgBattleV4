using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.EffectSystem.ActiveEffects;

public struct ActiveEffectContext
{
    public ISupportsEffects Holder;
    public Character Source;
    public ActiveEffectDefinition Definition;
    public uint Turns;
    public uint Stacks;
}