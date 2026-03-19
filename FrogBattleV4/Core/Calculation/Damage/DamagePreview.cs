using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.Calculation.Damage;

public record DamagePreview([NotNull] IBattleMember Target, double AverageExpectedDamage);