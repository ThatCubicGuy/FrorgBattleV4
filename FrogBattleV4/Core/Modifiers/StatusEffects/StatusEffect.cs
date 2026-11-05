using System;
using System.Collections.Immutable;
using System.Text.Json.Serialization;

namespace FrogBattleV4.Core.Modifiers.StatusEffects;

public class StatusEffect
{
    [JsonPropertyName("name")]
    public required string DisplayName { get; init; }
    public string? Description { get; init; }
    public int MaxStacks { get; init; } = 1;
    public int? MaxDuration { get; init; }
    public StatusEffectType Type { get; init; } = StatusEffectType.None;
    public StatusEffectFlags Properties { get; init; } = StatusEffectFlags.None;
    public ImmutableArray<IModifierRule> Modifiers { get; init; } = [];
    public ImmutableArray<IMutatorComponent> Mutators { get; init; } = [];
}

[JsonConverter(typeof(JsonStringEnumConverter<StatusEffectType>))]
public enum StatusEffectType
{
    [JsonStringEnumMemberName("none")]
    None,
    [JsonStringEnumMemberName("buff")]
    Buff,
    [JsonStringEnumMemberName("debuff")]
    Debuff,
}

[Flags]
public enum StatusEffectFlags
{
    None = 0,
    Unremovable = 1 << 0,
    Invisible = 1 << 1,
    Infinite = 1 << 2,
    StartTick = 1 << 3,
    RemoveStack = 1 << 4,
}