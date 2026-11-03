using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Modifiers.StatusEffects;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Registries;

// the stupid registry system
public class Registry<TKey, TValue>(FrozenDictionary<TKey, TValue> items)
    : IEnumerable<KeyValuePair<TKey, TValue>> where TKey : struct, IResourceId
{
    public TValue Get(TKey key) => TryGet(key, out var value)
        ? value
        : throw new KeyNotFoundException($"Key {key} does not exist in the registry.");

    public bool TryGet(TKey key, [MaybeNullWhen(false)] out TValue item) => items.TryGetValue(key, out item);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)items).GetEnumerator();
}

/// <summary>
/// Defines methods for creating registry loaders for different kinds of items.
/// </summary>
public static class DeferredRegistry
{
    private const bool SkipInvalidFiles = false;

    /// <summary>
    /// Creates a new shard registry factory.
    /// </summary>
    /// <param name="ns">Namespace for every shard in the registry.</param>
    /// <returns>A deferred shard register.</returns>
    public static Shards CreateShards(string ns) => new(ns);

    /// <summary>
    /// Creates a new fighter registry factory.
    /// </summary>
    /// <param name="ns">Namespace for every fighter in the registry.</param>
    /// <returns>A deferred shard register.</returns>
    public static Fighters CreateFighters(string ns) => new(ns);

    /// <summary>
    /// Creates a new status effect registry factory.
    /// </summary>
    /// <param name="ns">Namespace for every status effect in the registry.</param>
    /// <returns>A deferred status effect register.</returns>
    public static StatusEffects CreateStatusEffects(string ns) => new(ns);

    public class Shards : Loader<ShardId, Shard>
    {
        internal Shards(string ns) : base(ns, BaseDirectoryName)
        {
        }

        private static string BaseDirectoryName => "shards";
        protected override JsonTypeInfo<Shard> TypeInfo => AppSerializerContext.Default.Shard;

        public FrozenDictionary<ShardId, Shard> LoadAll()
        {
            return LoadAllFiles(BaseDirectoryName, SkipInvalidFiles).ToFrozenDictionary();
        }
    }

    public class Fighters : Loader<FighterId, Fighter>
    {
        internal Fighters(string ns) : base(ns, BaseDirectoryName)
        {
        }

        private static string BaseDirectoryName => "fighters";
        protected override JsonTypeInfo<Fighter> TypeInfo => AppSerializerContext.Default.Fighter;

        public FrozenDictionary<FighterId, Fighter> LoadAll()
        {
            return LoadAllFiles(BaseDirectoryName, SkipInvalidFiles).ToFrozenDictionary();
        }
    }

    public class StatusEffects : Loader<StatusEffectId, StatusEffect>
    {
        internal StatusEffects(string ns) : base(ns, BaseDirectoryName)
        {
        }

        private static string BaseDirectoryName => "status_effects";
        protected override JsonTypeInfo<StatusEffect> TypeInfo => AppSerializerContext.Default.StatusEffect;

        public FrozenDictionary<StatusEffectId, StatusEffect> LoadAll()
        {
            return LoadAllFiles(BaseDirectoryName, SkipInvalidFiles).ToFrozenDictionary();
        }
    }

    // TODO (Nova): Loaders shouldn't directly create a new registry if they are namespaced
    public abstract class Loader<TKey, TValue> where TKey : struct, IResourceId
    {
        private readonly Dictionary<TKey, TValue> _items = new();
        private readonly string _namespace;

        protected Loader(string ns, string baseDir)
        {
            _namespace = ns;
        }

        protected abstract JsonTypeInfo<TValue> TypeInfo { get; }

        /// <summary>
        /// Registers a value to the registry loader.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>This instance of loader.</returns>
        public Loader<TKey, TValue> Register(TKey key, TValue value)
        {
            _items.Add(key, value);
            return this;
        }

        /// <summary>
        /// Searches the base folder and loads every valid file found.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="skipInvalidFiles">Whether invalid files should be skipped or throw an exception.</param>
        /// <exception cref="JsonException">
        /// One of the files was invalid and <paramref name="skipInvalidFiles"/> is false.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// A duplicate ID was encountered after resolving filenames to resource IDs.
        /// </exception>
        protected Dictionary<TKey, TValue> LoadAllFiles(string directory, bool skipInvalidFiles = false)
        {
            var items = new Dictionary<TKey, TValue>();
            foreach (var file in Directory.EnumerateFiles(directory, "*", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var item = JsonSerializer.Deserialize(File.ReadAllText(file), TypeInfo)
                               ?? throw new JsonException($"Failed to load JSON file: {file}");
                    var key = GetKey(file);
                    if (!items.TryAdd(key, item))
                    {
                        throw new InvalidOperationException($"Duplicate resource ID: \"{key}\" (from file \"{file}\")");
                    }
                }
                catch (JsonException e)
                {
                    if (!skipInvalidFiles) throw;
                    Logger.WriteLine(e.Message);
                }
            }

            return items;
        }

        protected FrozenDictionary<TKey, TValue> GenerateFrozenDictionary() => _items.ToFrozenDictionary();

        /// <summary>
        /// Generates a registry key for an item with the given filename.
        /// </summary>
        /// <param name="file">Name of the file this item belongs to.</param>
        /// <returns>A registry key.</returns>
        private TKey GetKey(string file)
        {
            var filename = Path.GetFileNameWithoutExtension(file);

            return new TKey
            {
                Value = new ResourceId(_namespace, filename)
            };
        }
    }
}