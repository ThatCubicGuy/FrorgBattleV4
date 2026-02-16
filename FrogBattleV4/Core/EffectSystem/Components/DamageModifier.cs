#nullable enable
using FrogBattleV4.Core.EffectSystem.Modifiers;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.EffectSystem.Components;

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