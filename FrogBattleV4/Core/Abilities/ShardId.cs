using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Abilities;

[JsonConverter(typeof(ResourceIdConverter<ShardId>))]
public readonly record struct ShardId(ResourceId Value) : IResourceId
{
    public override string ToString() => Value.ToString();
    public static explicit operator ShardId(ResourceId value) => new(value);
    public static implicit operator ResourceId(ShardId value) => value.Value;
}