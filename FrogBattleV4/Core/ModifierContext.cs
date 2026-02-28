#nullable enable
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Effects.Modifiers;

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

public record ModifierQuery<TQuery>(TQuery Query, CalcDirection Direction) where TQuery : struct;

public record MutModifierQuery<TQuery>(TQuery Query, CalcDirection Direction, MutModifierDirection MutModifierDirection)
    : ModifierQuery<TQuery>(Query, Direction) where TQuery : struct;