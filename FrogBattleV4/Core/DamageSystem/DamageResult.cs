using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.DamageSystem;

/// <summary>
/// A fully calculated instance of damage. The raw value of <paramref name="Amount"/>
/// is deducted from the HP of the target.<br/>This record is mostly used for displays.
/// </summary>
/// <param name="ResultTarget">The target of the damage.</param>
/// <param name="Amount">The amount of damage taken.</param>
/// <param name="Type">The type of the damage dealt.</param>
/// <param name="IsCrit">Whether this damage instance is a critical hit.</param>
public record DamageResult(
    [NotNull] IDamageable ResultTarget,
    double Amount,
    [NotNull] string Type,
    bool IsCrit) : IResultContext<IDamageable>;

// Swap between implementations if one is more convenient than the other lmfao
file readonly struct OldDamageResult : IResultContext<IDamageable>
{
    [NotNull] public required IDamageable ResultTarget { get; init; }
    public required double Amount { get; init; }
    [NotNull] public required string Type { get; init; }
    public required bool IsCrit { get; init; }
}