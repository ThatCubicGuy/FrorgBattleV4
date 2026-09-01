using System.Collections.Immutable;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Abilities;


public class ShardLink
{
    public ShardLink(IEnumerable<AbilityShard> definitions)
    {
        Shards = [.. definitions];
        GenerateCommands();
    }

    public ImmutableList<AbilityShard> Shards { get; }

    public AbilityShard? this[string name] => Shards.SingleOrDefault(shard => shard.Name.Equals(name));

    public ShardLink Add(AbilityShard shard)
    {
        return new ShardLink(Shards.Append(shard));
    }

    private void GenerateCommands()
    {
        foreach (var shard in Shards)
        {
            shard.Components
        }
    }
}