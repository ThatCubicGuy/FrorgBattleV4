using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem;

public struct AbilityEligibilityContext
{
    public required Character User { get; init; }
    public required AbilityDefinition Definition { get; init; }
}