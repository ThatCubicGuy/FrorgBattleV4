using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.Effects.StatusEffects;

namespace FrogBattleV4.Core.Effects;

public record ApplyEffectCommand : IBattleCommand
{
    [NotNull] public required StatusEffectDefinition Definition { get; init; }
    [NotNull] public required IBattleMember Target { get; init; }
    [NotNull] public required System.Random Rng { get; init; }
    public IBattleMember EffectSource { get; init; }
    public double ApplicationChance { get; init; } = 1;
    public ChanceType ChanceType { get; init; } = ChanceType.Fixed;
    public int AddedStacks { get; init; } = 1;
    public required int InitialTurns { get; init; }
}

public enum ChanceType
{
    Fixed,
    Base
}