#nullable enable
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.DamageSystem;

public struct DamageQuery
{
    public string? Type { get; init; }
    public string? Source { get; init; }
    public required bool Crit { get; init; }
    public required ModifierDirection Direction { get; init; }
}