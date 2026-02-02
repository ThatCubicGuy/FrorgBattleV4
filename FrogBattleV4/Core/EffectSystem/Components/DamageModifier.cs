#nullable enable
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class DamageModifier : BasicModifierComponent<DamageCalcContext>
{
    public string? Type { get; init; }
    public string? Source { get; init; }
    public bool CritOnly { get; init; } = false;
    public required DamageModificationType ModificationType { get; init; }

    public override bool AppliesInContext(DamageCalcContext ctx)
    {
        return (!CritOnly || ctx.IsCrit) &&
               (Type ?? ctx.Type) == ctx.Type &&
               (Source ?? ctx.Source) == ctx.Source;
    }
}