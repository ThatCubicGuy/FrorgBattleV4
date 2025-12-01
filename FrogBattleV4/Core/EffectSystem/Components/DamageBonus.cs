#nullable enable

namespace FrogBattleV4.Core.EffectSystem.Components;

public class DamageBonus : IModifierComponent
{
    public string? Type { get; init; }
    public string? Source { get; init; }
    public double Amount { get; init; }
    // In truth, damage modifiers don't "add" their value to the damage,
    // but we just consider them as a percentage based stat and thus add
    // the value of the modifier to zero. A tiny bit scuffed, but it's alright.
    public Operator Operator => Operator.AddValue;

    public string Stat => string.Join('.', nameof(DamageBonus), Type ?? "AllType", Source ?? "AllSource");
}