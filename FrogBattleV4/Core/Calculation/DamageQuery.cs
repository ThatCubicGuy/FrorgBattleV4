#nullable enable
namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Mutation query classifying damage.
/// </summary>
public record DamageQuery : QueryBase
{
    public DamageType Type { get; init; }
    public DamageSource Source { get; init; }
    public required bool Crit { get; init; }
}