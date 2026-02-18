using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.BattleSystem.Actions;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core;

public class BattleMember : IBattleMember
{
    [NotNull] public required string Name { get; init; }
    [NotNull] public required ITargetable Hitbox { get; init; }
    [NotNull] public required IEnumerable<IAction> Turns { get; init; } = [];
    [NotNull] public IEnumerable<AbilityDefinition> Abilities { get; init; } = [];
    public void TakeDamage(DamageResult dmg)
    {
        var pool = this.GetPoolsByTag(PoolTag.AbsorbsDamage).LastOrDefault() ??
                   this.GetPoolsByTag(PoolTag.UsedForLife).LastOrDefault() ??
                   throw new NotSupportedException("You are not able to damage this member.");
        pool.CurrentValue -= dmg.Amount;
    }

    [NotNull] public required FrozenDictionary<StatId, double> BaseStats { get; init; }

    [NotNull] public EffectContainer Effects { get; } = new();
    // ReSharper disable once UseCollectionExpression cuz cmon
    [NotNull] public PoolContainer Pools { get; } = new();
}

#nullable enable
/// <summary>
/// Linked battle member with optional stats. Anything initialized is used,
/// otherwise parent's stats are used.
/// </summary>
/// <param name="parent">Member whose stats this one falls back on if they're missing.</param>
public class LinkedBattleMember(string name, IBattleMember parent) : IBattleMember
{
    public string Name { get; } = name;
    public IBattleMember Parent { get; } = parent;
    public IEnumerable<IAction>? OwnTurns { get; init; }
    public ITargetable? OwnHitbox { get; init; }
    public EffectContainer? OwnEffects { get; init; }
    public FrozenDictionary<StatId, double>? OwnBaseStats { get; init; }
    public PoolContainer? OwnPools { get; init; }
    public IEnumerable<AbilityDefinition>? OwnAbilities { get; init; }

    IEnumerable<IAction> ITakesTurns.Turns => OwnTurns ?? Parent.Turns;
    ITargetable IDamageable.Hitbox => OwnHitbox ?? Parent.Hitbox;
    EffectContainer EffectSystem.ISupportsEffects.Effects => OwnEffects ?? Parent.Effects;
    FrozenDictionary<StatId, double> IBattleMember.BaseStats => OwnBaseStats ?? Parent.BaseStats;
    PoolContainer IBattleMember.Pools => OwnPools ?? Parent.Pools;
    IEnumerable<AbilityDefinition> IHasAbilities.Abilities => OwnAbilities ?? Parent.Abilities;
    // Unsure how to go about making a method optional in data driven code...
    void IDamageable.TakeDamage(DamageResult dmg) => Parent.TakeDamage(dmg);
}