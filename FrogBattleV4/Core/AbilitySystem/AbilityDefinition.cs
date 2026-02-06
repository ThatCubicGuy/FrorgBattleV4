#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.AbilitySystem.Components;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.AbilitySystem;

public class AbilityDefinition
{
    #region Metadata

    /// <summary>
    /// A preferably unique string-based identifier for this ability.
    /// </summary>
    public required string Id { get; init; }
    /// <summary>
    /// The type of targets that the attacker is allowed to select for executing this ability on.
    /// </summary>
    public TargetingType TargetingType { get; init; }

    #endregion

    #region Components

    /// <summary>
    /// This ability's base targeting. Underlying components fall back to this if they don't have their own targeting.
    /// </summary>
    public ITargetingComponent? Targeting { get; init; }
    /// <summary>
    /// Prerequisites for casting the ability.
    /// </summary>
    public IEnumerable<IRequirementComponent> Requirements { get; init; } = [];
    /// <summary>
    /// Ability wide modifiers to be included in calculations.
    /// </summary>
    public IEnumerable<IModifierComponent> Modifiers { get; init; } = [];
    /// <summary>
    /// The costs of the ability, taxed after successful casting.
    /// </summary>
    public IEnumerable<ICostComponent> Costs { get; init; } = [];
    /// <summary>
    /// Every attack that this ability causes in order to damage others.
    /// </summary>
    public IEnumerable<IAttackComponent> Attacks { get; init; } = [];
    /// <summary>
    /// An array of effects that this ability might apply to the targets.
    /// </summary>
    public IEnumerable<IEffectComponent> Effects { get; init; } = [];

    #endregion
}