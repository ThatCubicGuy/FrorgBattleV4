using System.Collections.Frozen;
using System.Text.Json.Serialization.Metadata;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Registries;

public class FighterRegistry(FrozenDictionary<FighterId, Fighter> fighters) : Registry<FighterId, Fighter>(fighters)
{
    private const string BaseFighterDirectoryName = "characters";

    /// <summary>
    /// Gets the global singleton instance of the fighter registry.
    /// </summary>
    public static readonly FighterRegistry Global = new(FrozenDictionary<FighterId, Fighter>.Empty);

    public class Loader(string ns) : RegistryLoader(ns)
    {
        protected override string BaseDirectoryName => BaseFighterDirectoryName;
        protected override JsonTypeInfo<Fighter> TypeInfo => AppSerializerContext.Default.Fighter;
    }
}