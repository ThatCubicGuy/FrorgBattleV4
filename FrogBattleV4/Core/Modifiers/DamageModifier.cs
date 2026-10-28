using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

public class DamageModifier : MutationModifier<DamageQuery>
{
    [JsonPropertyName("damage")]
    public required DamageData Data { get; init; }
    public bool CritOnly { get; init; }

    protected override bool AppliesTo(DamageQuery query)
    {
        return (!CritOnly || query.IsCrit) && query.Data.Matches(Data);
    }
}

public class DamageStatModifier : AttributeModifier<DamageStatQuery>
{
    public required DamageData Data { get; init; }
    public required DamageStatChannel Channel { get; init; }

    protected override bool AppliesTo(DamageStatQuery query)
    {
        return query.Channel == Channel && query.Data.Matches(Data);
    }
}