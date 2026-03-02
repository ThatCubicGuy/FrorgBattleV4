#nullable enable
using System.Collections.Generic;
using FrogBattleV4.Core.AbilitySystem.Components;

namespace FrogBattleV4.Core.AbilitySystem;

public class AbilityDefinition
{
    #region Metadata

    /// <summary>
    /// A preferably unique string-based identifier for this ability.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Description of what this ability does.
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// The type of targets that the attacker is allowed to select for executing this ability on.
    /// </summary>
    public required TargetingPool TargetingPool { get; init; }

    #endregion

    #region Components

    /// <summary>
    /// This ability's base targeting. Underlying components fall back to this if they don't have their own targeting.
    /// </summary>
    public required ITargetingComponent Targeting { get; init; }

    /// <summary>
    /// Every component of the ability.
    /// </summary>
    public List<IAbilityComponent> Components { get; } = [];

    #endregion
}