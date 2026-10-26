using System.Collections.Frozen;
using System.Text.Json.Serialization.Metadata;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Registries;

public sealed class ShardRegistry(FrozenDictionary<ShardId, Shard> shards) : Registry<ShardId, Shard>(shards)
{
    private const string BaseShardDirectoryName = "shards";

    /// <summary>
    /// Gets the global singleton instance of the shard registry.
    /// </summary>
    public static readonly ShardRegistry Global = new(FrozenDictionary<ShardId, Shard>.Empty);

    public class Loader(string ns) : RegistryLoader(ns)
    {
        protected override string BaseDirectoryName => BaseShardDirectoryName;
        protected override JsonTypeInfo<Shard> TypeInfo => AppSerializerContext.Default.Shard;
    }
}