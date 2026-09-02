using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Abilities;

public class ShardLink(IEnumerable<IShard> definitions)
{
    public ImmutableList<IShard> Shards { get; } = [.. definitions];

    public ShardLink Link(IShard shard)
    {
        return new ShardLink(Shards.Append(shard));
    }

    private LinkResolution GenerateCommands(EntityUid user, EntityUid selectedTarget, BattleEnvironment env)
    {
        var state = new LinkResolutionState(user, new TargetingSelection([new AbilityTargetingContext
        {
            Target = selectedTarget,
            Rank = 0,
        }]), ModifierCollection.Empty, -1);
        var builder = new LinkResolutionBuilder();

        foreach (var shard in Shards)
        {
            state = state with
            {
                CurrentShardIndex = state.CurrentShardIndex + 1,
            };
            shard.Resolve(ref state, env, builder);
        }

        return builder.Build();
    }
}