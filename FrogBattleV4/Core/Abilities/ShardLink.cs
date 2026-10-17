using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Abilities;

public class ShardLink
{
    private readonly List<IShard> _shards;

    public ShardLink(IEnumerable<IShard> shards)
    {
        _shards = [.. shards];
        Shards = new ReadOnlyCollection<IShard>(_shards);
    }

    public ReadOnlyCollection<IShard> Shards { get; }

    public void Link(IShard shard)
    {
        if (_shards.Contains(shard))
        {
            throw new InvalidOperationException("The shard is already linked.");
        }
        _shards.Add(shard);
    }

    public void Unlink(IShard shard)
    {
        if (!_shards.Contains(shard))
        {
            throw new InvalidOperationException("The shard is not linked.");
        }
        _shards.Remove(shard);
    }

    private LinkResolution GenerateActions(EntityUid user, EntityUid selectedTarget, BattleEnvironment env)
    {
        var state = new LinkResolutionState(user, [new AbilityTargetingContext
        {
            Target = selectedTarget,
            Rank = 0,
        }], ModifierCollection.Empty, 0);
        var builder = new LinkResolutionBuilder();

        foreach (var shard in Shards)
        {
            shard.Resolve(ref state, env, builder);
            state = state with
            {
                CurrentShardIndex = state.CurrentShardIndex + 1,
            };
        }

        return builder.Build();
    }
}