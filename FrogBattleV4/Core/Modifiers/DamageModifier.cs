using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers.Components;

public class DamageModifier : MutationModifier<DamageQuery>
{
    public required DamageData Data { get; init; }
    public bool CritOnly { get; init; }
    protected override bool AppliesTo(DamageQuery query)
    {
        return (!CritOnly || query.Crit) && query.Data == Data;
    }
}