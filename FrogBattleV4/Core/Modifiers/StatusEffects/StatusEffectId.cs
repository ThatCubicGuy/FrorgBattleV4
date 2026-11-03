using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

[JsonConverter(typeof(ResourceIdConverter<StatusEffectId>))]
public readonly record struct StatusEffectId(ResourceId Value) : IResourceId
{
    public override string ToString() => Value.ToString();
    public static explicit operator StatusEffectId(ResourceId value) => new(value);
    public static implicit operator ResourceId(StatusEffectId value) => value.Value;
}