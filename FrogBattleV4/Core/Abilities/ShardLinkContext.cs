using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Combat.Actions;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Abilities;

/// <summary>
/// A mutable context describing the current state of resolution.
/// </summary>
/// <param name="User">Entity using this link.</param>
/// <param name="Selections">Currently selected targets.</param>
/// <param name="Modifiers">Currently active modifiers.</param>
/// <param name="CurrentShardIndex">Index of the currently resolving shard</param>
public sealed record LinkResolutionState(
    EntityUid User,
    TargetingSelection Selections,
    ModifierCollection Modifiers,
    int CurrentShardIndex);

/// <summary>
/// Small scope object for a single shard command's resolution.
/// </summary>
/// <param name="User">User of the shard.</param>
/// <param name="Targeting">Target of the shard.</param>
/// <param name="Modifier">Modifiers owned by the user at time of resolution.</param>
public record ShardResolutionScope(
    EntityUid User,
    AbilityTargetingContext Targeting,
    IModifierProvider Modifier);

public sealed class LinkResolutionBuilder
{
    private readonly List<ShardAction> _shardActions = [];

    public void Emit(ShardAction shardAction) => _shardActions.Add(shardAction);

    [Pure]
    public LinkResolution Build() => new(_shardActions);
}

public class LinkResolution(IEnumerable<ShardAction> actions)
{
    public ImmutableList<ShardAction> ShardActions { get; init; } = [.. actions];
}