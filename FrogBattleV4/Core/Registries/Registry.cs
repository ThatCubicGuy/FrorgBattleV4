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

public abstract class Registry<TKey, TItems>(FrozenDictionary<TKey, TItems> items) : IEnumerable<KeyValuePair<TKey, TItems>> where TKey : struct, IResourceId
{
    private FrozenDictionary<TKey, TItems> _items = items;

    public TItems Get(TKey key) => TryGet(key, out var value)
        ? value
        : throw new KeyNotFoundException($"Key {key} does not exist in the registry.");

    public bool TryGet(TKey key, [MaybeNullWhen(false)] out TItems item) => _items.TryGetValue(key, out item);

    /// <summary>
    /// Replace the entire registry with the given value.
    /// </summary>
    /// <param name="items">New registry to store.</param>
    public void Replace(FrozenDictionary<TKey, TItems> items) => _items = items;

    public IEnumerator<KeyValuePair<TKey, TItems>> GetEnumerator() => _items.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_items).GetEnumerator();

    public abstract class RegistryLoader(string ns)
    {
        protected abstract string BaseDirectoryName { get; }
        protected abstract JsonTypeInfo<TItems> TypeInfo { get; }

        public FrozenDictionary<TKey, TItems> LoadAll(bool skipInvalidFiles = false)
        {
            var items = new Dictionary<TKey, TItems>();
            foreach (var file in Directory.EnumerateFiles(BaseDirectoryName, "*", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var item = JsonSerializer.Deserialize(File.ReadAllText(file), TypeInfo)
                                ?? throw new JsonException("Failed to load shard.");
                    var key = GetKey(file);
                    if (!items.TryAdd(key, item))
                    {
                        throw new InvalidOperationException($"Duplicate shard ID: \"{key}\" (from file \"{file}\")");
                    }
                }
                catch (JsonException)
                {
                    if (!skipInvalidFiles) throw;
                }
            }

            return items.ToFrozenDictionary();
        }

        private TKey GetKey(string file)
        {
            var filename = Path.GetFileNameWithoutExtension(file);

            return new TKey
            {
                Value = new ResourceId(ns, filename)
            };
        }
    }
}