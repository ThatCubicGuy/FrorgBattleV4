using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core;

/// <summary>
/// Contextual resource location indicating some manner of registry item.
/// </summary>
/// <param name="Namespace">Namespace of the resource ID; e.g. "frorgbattle".</param>
/// <param name="Path">Path of the resource ID; e.g. "rexulti".</param>
public readonly record struct ResourceId(string Namespace, string Path)
{
    public override string ToString() => $"{Namespace}:{Path}";
}

[JsonConverter(typeof(ResourceIdConverter<FighterId>))]
public readonly record struct FighterId(ResourceId Value) : IResourceId
{
    public override string ToString() => Value.ToString();
    public static explicit operator FighterId(ResourceId value) => new(value);
    public static implicit operator ResourceId(FighterId value) => value.Value;
}

[JsonConverter(typeof(ResourceIdConverter<ShardId>))]
public readonly record struct ShardId(ResourceId Value) : IResourceId
{
    public override string ToString() => Value.ToString();
    public static explicit operator ShardId(ResourceId value) => new(value);
    public static implicit operator ResourceId(ShardId value) => value.Value;
}