#nullable enable
using System.Collections.Frozen;
using System.Collections.Generic;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Combat.Actions;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core;

/// <summary>
/// Linked battle member with optional stats. Anything initialized is used,
/// otherwise parent's stats are used.
/// </summary>
/// <param name="parent">Member whose stats this one falls back on if they're missing.</param>
public class LinkedBattleMember(string name, IBattleMember parent) : IBattleMember
{
    public string Name { get; } = name;
    public IBattleMember Parent { get; } = parent;

    public IEnumerable<ScheduledAction>? OwnTurns { get; init; }
    public ITargetable? OwnHitbox { get; init; }
    public EffectContainer? OwnEffects { get; init; }
    public PoolContainer? OwnPools { get; init; }
    public FrozenDictionary<StatId, double>? OwnBaseStats { get; init; }
    public IEnumerable<AbilityDefinition>? OwnAbilities { get; init; }

    IEnumerable<ScheduledAction> ITakesTurns.Turns => OwnTurns ?? Parent.Turns;
    ITargetable IDamageable.Hitbox => OwnHitbox ?? Parent.Hitbox;
    EffectContainer IBattleMember.Effects => OwnEffects ?? Parent.Effects;
    PoolContainer IBattleMember.Pools => OwnPools ?? Parent.Pools;
    FrozenDictionary<StatId, double> IBattleMember.BaseStats => OwnBaseStats ?? Parent.BaseStats;
    IEnumerable<AbilityDefinition> IHasAbilities.Abilities => OwnAbilities ?? Parent.Abilities;
    // Unsure how to go about making a method optional in data driven code...
    void IDamageable.TakeDamage(DamageResult dmg) => Parent.TakeDamage(dmg);
}