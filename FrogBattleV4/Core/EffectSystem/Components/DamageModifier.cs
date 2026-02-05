#nullable enable
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class DamageModifier : IModifierRule<DamageQuery>
{
    public required ModifierStack ModifierStack { get; init; } = new();
    public string? Type { get; init; }
    public string? Source { get; init; }
    public bool CritOnly { get; init; } = false;
    public required ModifierDirection Direction { get; init; }

    public bool AppliesFor(DamageQuery query)
    {
        return Direction == query.Direction &&
               (!CritOnly || query.Crit) &&
               (Type ?? query.Type) == query.Type &&
               (Source ?? query.Source) == query.Source;
    }
}