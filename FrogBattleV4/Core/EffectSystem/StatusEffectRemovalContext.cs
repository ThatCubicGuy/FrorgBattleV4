#nullable enable
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.EffectSystem;

public struct StatusEffectRemovalContext()
{
    public required string EffectId { get; init; }
    public Character Source { get; init; }
    public ISupportsEffects Target { get; init; }
    public double RemovalChance { get; init; } = 1;
    public uint RemovedStacks { get; init; } = 1;
    public uint RemovedTurns { get; init; } = 0;
    public required System.Random Rng { get; init; }
}