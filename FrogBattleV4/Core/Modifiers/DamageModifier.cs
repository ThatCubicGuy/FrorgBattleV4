using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

public class DamageModifier : MutationModifier<DamageQuery>
{
    [JsonPropertyName("damage")]
    public DamageMetadata Metadata { get; init; } = DamageMetadata.Default;
    public bool CritOnly { get; init; }

    protected override bool AppliesTo(DamageQuery query)
    {
        return (!CritOnly || query.IsCrit) && Metadata.Permits(query.Metadata);
    }
}

public class DamagePropertyModifier : AttributeModifier<DamageStatQuery>
{
    [JsonPropertyName("damage")]
    public required DamageMetadata Metadata { get; init; }
    public required DamageStatChannel Channel { get; init; }

    protected override bool AppliesTo(DamageStatQuery query)
    {
        return query.Channel == Channel && Metadata.Permits(query.Metadata);
    }
}

[JsonConverter(typeof(JsonStringEnumConverter<DamageStatChannel>))]
public enum DamageStatChannel
{
    [JsonStringEnumMemberName("crit_rate")]
    CritRate = 1,
    // Leaves potential for adding things like HitRate and whatnot
}