using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Combat.Actions;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core;

public class BattleMember : IBattleMember
{
    [NotNull] public required string Name { get; init; }
    [NotNull] public required ITargetable Hitbox { get; init; }
    [NotNull] public required IEnumerable<ScheduledAction> Turns { get; init; } = [];
    [NotNull] public IEnumerable<AbilityDefinition> Abilities { get; init; } = [];
    public void TakeDamage(DamageResult dmg)
    {
        var pool = this.GetPoolsByTag(PoolTag.AbsorbsDamage).LastOrDefault() ??
                   this.GetPoolsByTag(PoolTag.UsedForLife).LastOrDefault() ??
                   throw new NotSupportedException("You are not able to damage this member.");
        pool.CurrentValue -= dmg.Amount;
    }

    [NotNull] public required StatContainer BaseStats { get; init; }

    [NotNull] public EffectContainer Effects { get; } = new();
    // ReSharper disable once UseCollectionExpression cuz cmon
    [NotNull] public PoolContainer Pools { get; } = new();
}