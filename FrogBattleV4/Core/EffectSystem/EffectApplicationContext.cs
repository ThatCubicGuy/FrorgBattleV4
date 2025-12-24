#nullable enable
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;

namespace FrogBattleV4.Core.EffectSystem;

public struct EffectApplicationContext
{
    public ICharacter Source;
    public ISupportsEffects Target;
    public ActiveEffectDefinition Definition;
    public double ApplicationChance;
    public ChanceType ChanceType;
    public System.Random Rng;
    public uint InitialTurns;
    public uint InitialStacks;
}

public enum ChanceType
{
    Fixed,
    Base
}