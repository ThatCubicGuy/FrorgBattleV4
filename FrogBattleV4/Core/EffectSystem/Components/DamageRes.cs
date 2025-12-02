#nullable enable

namespace FrogBattleV4.Core.EffectSystem.Components;

public class DamageRes : IModifierComponent
{
    public string? Type { get; init; }
    public string? Source { get; init; }
    public required double Amount { get; init; }

    public bool Modifies(string key, EffectContext? ctx = null)
    {
        return key == string.Join('.', nameof(DamageBonus), Type ?? "AllType", Source ?? "AllSource");
    }

    public double Apply(double currentValue, EffectContext ctx) => currentValue * Amount;
}