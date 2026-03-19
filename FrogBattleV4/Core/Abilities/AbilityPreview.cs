#nullable enable
using FrogBattleV4.Core.Abilities.Components;

namespace FrogBattleV4.Core.Abilities;

public record AbilityPreview
{
    public required AbilityDefinition Definition { get; init; }
    public required bool CanUse { get; init; }
    public required IBattleCommand[] Commands { get; init; }
    public required IAbilityRequirementComponent[] UnfulfilledRequirements { get; init; }
}