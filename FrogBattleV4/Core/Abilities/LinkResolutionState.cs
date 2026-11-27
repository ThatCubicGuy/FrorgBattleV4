using System.Collections.Generic;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Abilities;

/// <summary>
/// A context describing the current state of resolution.
/// </summary>
/// <param name="User">Entity using this link.</param>
/// <param name="Selections">Currently selected targets.</param>
/// <param name="Modifiers">Modifiers owned by the user at time of resolution.</param>
/// <param name="CurrentShardIndex">Index of the currently resolving shard.</param>
public sealed record LinkResolutionState(
    EntityUid User,
    IEnumerable<TargetingContext> Selections,
    IEnumerable<IModifierProvider> Modifiers,
    int CurrentShardIndex);