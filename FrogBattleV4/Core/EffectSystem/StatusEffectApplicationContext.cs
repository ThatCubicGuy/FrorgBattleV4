#nullable enable
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.EffectSystem.StatusEffects;

namespace FrogBattleV4.Core.EffectSystem;

public struct StatusEffectApplicationContext()
{
    public required StatusEffectDefinition Definition { get; init; }
    public BattleMember Source { get; init; }
    public ISupportsEffects Target { get; init; }
    public double ApplicationChance { get; init; } = 1;
    public ChanceType ChanceType { get; init; } = ChanceType.Fixed;
    public uint InitialStacks { get; init; } = 1;
    public required uint InitialTurns { get; init; }
    public required System.Random Rng { get; init; }
}

public enum ChanceType
{
    Fixed,
    Base
}