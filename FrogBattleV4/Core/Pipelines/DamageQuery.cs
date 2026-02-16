#nullable enable
namespace FrogBattleV4.Core.Pipelines;

/// <summary>
/// Mutation query classifying damage.
/// </summary>
public struct DamageQuery
{
    public DamageType Type { get; init; }
    public DamageSource Source { get; init; }
    public required bool Crit { get; init; }
}