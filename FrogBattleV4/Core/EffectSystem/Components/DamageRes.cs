#nullable enable
using System.Linq;

namespace FrogBattleV4.Core.EffectSystem.Components;

public class DamageRes : IModifierComponent
{
    public string? Type { get; init; }
    public string? Source { get; init; }
    public double Amount { get; init; }

    public string GetKey() => string.Join('.', nameof(DamageRes), Type ?? "AllType", Source ?? "AllSource");
}