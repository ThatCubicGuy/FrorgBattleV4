#nullable enable
namespace FrogBattleV4.Core.DamageSystem;

public struct DamageRequest
{
    public string? Type { get; init; }
    public string? Source { get; init; }
    public required bool Crit { get; init; }
}