#nullable enable
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class DamageModifier : BasicModifierComponent<DamageCalcContext>, IDamageModifier
{
    public string? Type { get; init; }
    public string? Source { get; init; }
    public required DamageModificationType ModificationType { get; init; }

    public override bool AppliesInContext(DamageCalcContext ctx)
    {
        return (Type ?? ctx.Type) == ctx.Type &&
            (Source ?? ctx.Source) == ctx.Source;
    }
}