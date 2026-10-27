using System.Collections.Frozen;
using FrogBattleV4.Core.Abilities;

namespace FrogBattleV4.Core.Registries;

public class ShardRegistry(FrozenDictionary<ShardId, Shard> items) : Registry<ShardId, Shard>(items)
{
    public static readonly ShardRegistry Global = RegistryLoader.CreateShards("frorgbattle").Load();
}