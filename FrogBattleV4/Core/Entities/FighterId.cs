using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Entities;

[JsonConverter(typeof(ResourceIdConverter<FighterId>))]
public readonly record struct FighterId(ResourceId Value) : IResourceId
{
    public override string ToString() => Value.ToString();
    public static explicit operator FighterId(ResourceId value) => new(value);
    public static implicit operator ResourceId(FighterId value) => value.Value;
}