using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Abilities;

/// <summary>
/// A mutable context describing the current, unresolved state of linking.
/// </summary>
public class ShardLinkContext
{
    public required FighterBase User { get; init; }
    public required BattleState State { get; init; }
    /// <summary>
    /// The currently selected shards.
    /// </summary>
    public ImmutableList<IShard> CurrentLink { get; private set; } = [];

    /// <summary>
    /// Target the user's crosshair is aiming at.
    /// </summary>
    public required GameEntity SelectedTarget { get; init; }

    public ImmutableList<AbilityTargetingContext> Targets { get; private set; } = [];

    public required Random Rng { get; init; }

    public void Link(IShard shard)
    {
        CurrentLink = CurrentLink.Add(shard);
    }
    public void SetTargets(IEnumerable<AbilityTargetingContext> targets)
    {
        Targets = [.. targets];
    }
}

public class LinkResolutionBuilder
{
    private readonly List<IShardAction> _shardActions = [];

    public void Add(IShardAction shardAction)
    {
        _shardActions.Add(shardAction);
    }

    public void Remove(IShardAction shardAction)
    {
        _shardActions.Remove(shardAction);
    }

    public void Undo()
    {
        _shardActions.RemoveAt(_shardActions.Count - 1);
    }

    public LinkResolution Build()
    {
        return new LinkResolution(_shardActions);
    }
}

public class LinkResolution(IEnumerable<IShardAction> actions)
{
    public ImmutableList<IShardAction> ShardActions { get; init; } = [.. actions];
}