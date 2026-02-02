#nullable enable
using FrogBattleV4.Core.AbilitySystem.Components;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.AbilitySystem;

public struct AbilityPreview
{
    public required bool CanUse { get; init; }
    public required DamagePreview[]? Damages { get; init; }
    public required MutationResult[]? Mutations { get; init; }
    public required IRequirementComponent[]? UnfulfilledRequirements { get; init; }
}