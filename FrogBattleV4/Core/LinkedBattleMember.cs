#nullable enable
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;

namespace FrogBattleV4.Core;

/// <summary>
/// Linked battle member with optional stats. Anything initialized is used,
/// otherwise parent's stats are used.
/// </summary>
public class LinkedBattleMember(IBattleMember parent) : IBattleMember
{
    public required string Name { get; init; }
    public IBattleMember Parent { get; } = parent;

    public ITargetable? OwnHitbox { get; init; }
    public TurnContainer? OwnTurn { get; init; }
    public AbilityContainer? OwnAbilities { get; init; }
    public EffectContainer? OwnEffects { get; init; }
    public PoolContainer? OwnPools { get; init; }
    public StatContainer? OwnStats { get; init; }

    ITargetable IBattleMember.Hitbox => OwnHitbox ?? Parent.Hitbox;
    TurnContainer IBattleMember.Turn => OwnTurn ?? Parent.Turn;
    AbilityContainer IBattleMember.Abilities => OwnAbilities ?? Parent.Abilities;
    EffectContainer IBattleMember.Effects => OwnEffects ?? Parent.Effects;
    PoolContainer IBattleMember.Pools => OwnPools ?? Parent.Pools;
    StatContainer IBattleMember.BaseStats => OwnStats ?? Parent.BaseStats;
}