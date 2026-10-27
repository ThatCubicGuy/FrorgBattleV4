using System;
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Registries;

public class Registry<TKey, TValue>(FrozenDictionary<TKey, TValue> items) : IEnumerable<KeyValuePair<TKey, TValue>> where TKey : struct, IResourceId
{
    private FrozenDictionary<TKey, TValue> _items = items;

    public TValue Get(TKey key) => TryGet(key, out var value)
        ? value
        : throw new KeyNotFoundException($"Key {key} does not exist in the registry.");

    public bool TryGet(TKey key, [MaybeNullWhen(false)] out TValue item) => _items.TryGetValue(key, out item);

    /// <summary>
    /// Replace the entire registry with the given value.
    /// </summary>
    /// <param name="items">New registry to store.</param>
    public void Replace(FrozenDictionary<TKey, TValue> items) => _items = items;

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_items).GetEnumerator();
}

/// <summary>
/// Defines methods for creating registry loaders for different kinds of items.
/// </summary>
public static class RegistryLoader
{
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

    public class Shards : Loader<ShardId, Abilities.Shard>
    {
        internal Shards(string ns) : base(ns)
        {
        }

        protected override string BaseDirectoryName => "shards";
        protected override JsonTypeInfo<Abilities.Shard> TypeInfo => AppSerializerContext.Default.Shard;

        public override ShardRegistry Load()
        {
            return new ShardRegistry(GenerateFrozenDictionary());
        }
    }

    public class Fighters : Loader<FighterId, Entities.Fighter>
    {
        internal Fighters(string ns) : base(ns)
        {
        }

        protected override string BaseDirectoryName => "fighters";
        protected override JsonTypeInfo<Entities.Fighter> TypeInfo => AppSerializerContext.Default.Fighter;

        public override FighterRegistry Load() => new(GenerateFrozenDictionary());
    }

    public abstract class Loader<TKey, TValue> where TKey : struct, IResourceId
    {
        private readonly Dictionary<TKey, TValue> _items = new();
        private readonly string _namespace;

        protected Loader(string ns)
        {
            _namespace = ns;
        }

        protected abstract string BaseDirectoryName { get; }
        protected abstract JsonTypeInfo<TValue> TypeInfo { get; }

        /// <summary>
        /// Loads a new registry with the values registered so far in this loader.
        /// </summary>
        /// <returns>A new registry.</returns>
        public abstract Registry<TKey, TValue> Load();

        /// <summary>
        /// Registers a value to the registry loader.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Loader<TKey, TValue> Register(TKey key, TValue value)
        {
            _items.Add(key, value);
            return this;
        }

        /// <summary>
        /// Searches the base folder and loads every valid file found.
        /// </summary>
        /// <param name="skipInvalidFiles">Whether invalid files should be skipped or throw an exception.</param>
        /// <exception cref="JsonException">One of the files was invalid and <paramref name="skipInvalidFiles"/> is false.</exception>
        /// <exception cref="InvalidOperationException">A duplicate shard was encountered after resolving filenames to resource IDs.</exception>
        public void ReloadAllFiles(bool skipInvalidFiles = true)
        {
            _items.Clear();
            foreach (var file in Directory.EnumerateFiles(BaseDirectoryName, "*", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var item = JsonSerializer.Deserialize(File.ReadAllText(file), TypeInfo)
                               ?? throw new JsonException($"Failed to load JSON file: {file}");
                    var key = GetKey(file);
                    if (!_items.TryAdd(key, item))
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