#nullable enable
using FrogBattleV4.Core.AbilitySystem.Components;

namespace FrogBattleV4.Core.AbilitySystem;

public struct AbilityPreview
{
    public required bool CanUse { get; init; }
    public required IBattleCommand[] Commands { get; init; }
    public required IAbilityRequirementComponent[] UnfulfilledRequirements { get; init; }
}