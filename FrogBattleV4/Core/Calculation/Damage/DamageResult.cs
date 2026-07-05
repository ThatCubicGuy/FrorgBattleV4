using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.Contexts;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Calculation.Damage;

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