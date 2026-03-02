using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Contexts;

namespace FrogBattleV4.Core.DamageSystem;

// Swap between the more convenient implementation lmao
/// <summary>
/// A fully calculated instance of damage. The raw value of <paramref name="Amount"/>
/// is deducted from the HP of the target.<br/>This record is mostly used for displays.
/// </summary>
/// <param name="Amount">The amount of damage taken.</param>
/// <param name="ResultTarget">The target of the damage.</param>
/// <param name="Type">The type of the damage dealt.</param>
/// <param name="IsCrit">Whether this damage instance is a critical hit.</param>
public record DamageResult(
    double Amount,
    [NotNull] IBattleMember ResultTarget,
    DamageType Type,
    bool IsCrit) : IResultContext<IBattleMember>;

/// <summary>
/// A fully calculated instance of damage. The raw value of <see cref="Amount"/>
/// is deducted from the HP of the target.<br/>This record is mostly used for displays.
/// </summary>
file readonly struct OldDamageResult : IResultContext<IBattleMember>
{
    /// <summary>
    /// The amount of damage taken.
    /// </summary>
    public required double Amount { get; init; }
    /// <summary>
    /// The target of the damage.
    /// </summary>
    [NotNull] public required IBattleMember ResultTarget { get; init; }
    /// <summary>
    /// The type of the damage dealt.
    /// </summary>
    public required DamageType Type { get; init; }
    /// <summary>
    /// Whether this damage instance is a critical hit.
    /// </summary>
    public required bool IsCrit { get; init; }
}