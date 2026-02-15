#nullable enable
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class DamageModifier : ModifierRule<DamageRequest>
{
    public string? Type { get; init; }
    public string? Source { get; init; }
    public bool CritOnly { get; init; }

    protected override bool AppliesToRequest(DamageRequest request)
    {
        return (!CritOnly || request.Crit) &&
               (Type ?? request.Type) == request.Type &&
               (Source ?? request.Source) == request.Source;
    }
}