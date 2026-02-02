using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.DamageSystem;

public record DamagePreview([NotNull] IDamageable Target, double PotentialMinDamage, double PotentialMaxDamage);