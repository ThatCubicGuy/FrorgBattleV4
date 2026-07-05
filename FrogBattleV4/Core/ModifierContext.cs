#nullable enable
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Effects.Modifiers;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core;

/// <summary>
/// Context that funnels everything about modifiers into one place.
/// </summary>
public readonly record struct ModifierContext(
    // The actor in this context.
    IBattleMember? Actor = null,
    // Member we take as reference.
    IBattleMember? Other = null,
    // Ability being used.
    AbilityDefinition? Ability = null,
    // Targeting type in case we attack the reference.
    TargetingType? Aiming = null,
    // Rng for those who need it.
    System.Random? Rng = null);

public record ModifierQuery(QueryBase Query, CalcDirection Direction);

public record MutModifierQuery(QueryBase Query, CalcDirection Direction, MutModifierDirection MutModifierDirection)
    : ModifierQuery(Query, Direction);