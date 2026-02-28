#nullable enable
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Effects.Modifiers;

namespace FrogBattleV4.Core.Effects.Components;

public class DamageMutModifier : MutModifierRule<DamageQuery>
{
    public DamageType Type { get; init; }
    public DamageSource Source { get; init; }
    public bool CritOnly { get; init; }

    protected override bool AppliesToQuery(DamageQuery query)
    {
        return (!CritOnly || query.Crit) &&
               Type.Matches(query.Type) &&
               Source.Matches(query.Source);
    }
}