#nullable enable
using FrogBattleV4.Core.AbilitySystem.Components;

namespace FrogBattleV4.Core.AbilitySystem;

public class AbilityDefinition
{
    // Metadata
    public required string Id { get; init; }
    public TargetingTypes TargetingType { get; init; }
    // Components
    public ITargetingComponent? Targeting { get; init; }
    public IRequirementComponent[] Requirements { get; init; } = [];
    public ICostComponent[] Costs { get; init; } = [];
    public IAttackComponent[] Attacks { get; init; } = [];
    public IEffectComponent[] Effects { get; init; } = [];
}