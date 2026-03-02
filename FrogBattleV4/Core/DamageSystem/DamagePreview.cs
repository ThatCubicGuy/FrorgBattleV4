using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.DamageSystem;

public record DamagePreview([NotNull] IBattleMember Target, double PotentialNonCritDamage, double PotentialCritDamage);