#nullable enable
using FrogBattleV4.Core.AbilitySystem.Components;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.AbilitySystem;

public class AbilityDefinition
{
    // Metadata
    public required string Id { get; init; }
    public TargetingType TargetingType { get; init; }
    // Components
    public ITargetingComponent? Targeting { get; init; }
    public IRequirementComponent[]? Requirements { get; init; }
    public IModifierComponent[]? Modifiers { get; init; }
    public ICostComponent[]? Costs { get; init; }
    public IAttackComponent[]? Attacks { get; init; }
    public IEffectComponent[]? Effects { get; init; }
}