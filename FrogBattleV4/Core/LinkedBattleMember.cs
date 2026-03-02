#nullable enable
using System.Collections.Generic;
using System.Linq;
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
public class LinkedBattleMember : IBattleMember
{
    public LinkedBattleMember(IBattleMember parent)
    {
        Parent = parent;
        Components.ComponentAdded += RegisterCache;
    }
    public required string Name { get; init; }
    public IBattleMember Parent { get; }

    public ComponentContainer Components { get; } = [];
    public IEnumerable<ScheduledAction>? OwnTurns { get; init; }
    public ITargetable? OwnHitbox { get; init; }
    public AbilityContainer? OwnAbilities { get; private set; }
    public EffectContainer? OwnEffects { get; private set; }
    public PoolContainer? OwnPools { get; private set; }
    public StatContainer? OwnStats { get; private set; }

    private void RegisterCache(IBattleMemberComponent component)
    {
        switch (component)
        {
            case AbilityContainer container:
                OwnAbilities = container;
                break;
            case EffectContainer container:
                OwnEffects = container;
                break;
            case PoolContainer container:
                OwnPools = container;
                break;
            case StatContainer container:
                OwnStats = container;
                break;
        }
    }

    IEnumerable<ScheduledAction> ITakesTurns.Turns => OwnTurns ?? Parent.Turns;
    ITargetable IBattleMember.Hitbox => OwnHitbox ?? Parent.Hitbox;
    AbilityContainer IBattleMember.Abilities => OwnAbilities ?? Parent.Abilities;
    EffectContainer IBattleMember.Effects => OwnEffects ?? Parent.Effects;
    PoolContainer IBattleMember.Pools => OwnPools ?? Parent.Pools;
    StatContainer IBattleMember.BaseStats => OwnStats ?? Parent.BaseStats;
}