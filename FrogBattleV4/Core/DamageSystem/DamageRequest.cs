using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.EffectSystem.Modifiers;

namespace FrogBattleV4.Core.DamageSystem;

public readonly struct DamageRequest()
{
    public required double BaseAmount { get; init; }
    [NotNull] public required IDamageable Target { get; init; }
    [NotNull] public required DamageProperties Properties { get; init; }
    [NotNull] public IEnumerable<IModifierRule> ExtraModifiers { get; init; } = [];
    public bool CanCrit { get; init; } = true;
}