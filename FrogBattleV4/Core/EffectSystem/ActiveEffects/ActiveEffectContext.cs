using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.EffectSystem.ActiveEffects;

public struct ActiveEffectContext : IContext
{
    public ICharacter Holder;
    public ICharacter Source;
    public ActiveEffectDefinition Definition;
    public uint Turns;
    public uint Stacks;
}