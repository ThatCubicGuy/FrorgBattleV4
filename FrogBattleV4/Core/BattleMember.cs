#nullable enable
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.BattleSystem.Actions;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Modifiers;
using FrogBattleV4.Core.EffectSystem.PassiveEffects;
using FrogBattleV4.Core.EffectSystem.StatusEffects;
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core;

public abstract class BattleMember : ITakesTurns, IHasStats, IHasPools, ISupportsEffects
{
    private readonly List<StatusEffectInstance> _activeEffects = [];
    private readonly List<PassiveEffectDefinition> _passiveEffects = [];
    private readonly Dictionary<PoolId, PoolComponent> _pools;

    public event EventHandler<StatusEffectApplicationContext>? EffectApplySuccess;
    public event EventHandler<StatusEffectApplicationContext>? EffectApplyFailure;
    public event EventHandler<StatusEffectRemovalContext>? EffectRemoveSuccess;
    public event EventHandler<StatusEffectRemovalContext>? EffectRemoveFailure;

    protected BattleMember()
    {
        _pools = new Dictionary<PoolId, PoolComponent>();
        Pools = new ReadOnlyDictionary<PoolId, PoolComponent>(_pools);
    }

    [NotNull] public string? Name { get; protected init; }
    [NotNull] public IEnumerable<IAction> Turns { get; protected init; } = [];

    [NotNull] public ITargetable Hitbox { get; protected init; }
    [NotNull] public IReadOnlyDictionary<PoolId, PoolComponent> Pools { get; }
    public bool AddPool(PoolComponent pool) => _pools.TryAdd(pool.Id, pool);
    public bool RemovePool(PoolId poolId) => _pools.Remove(poolId);

    [NotNull] public IEnumerable<IModifierComponent> AttachedEffects =>
        _passiveEffects.Concat<IModifierComponent>(_activeEffects);
    [NotNull] protected IEnumerable<StatusEffectInstance> ActiveEffects => _activeEffects;

    [NotNull] public IEnumerable<PassiveEffectDefinition> PassiveEffects
    {
        protected get => _passiveEffects;
        init => _passiveEffects = value.ToList();
    }

    protected void ForceAddActive(StatusEffectInstance effect) => _activeEffects.Add(effect);
    protected void ForceRemoveActive(StatusEffectInstance effect) => _activeEffects.Remove(effect);
    protected void ForceAddPassive(PassiveEffectDefinition passive) => _passiveEffects.Add(passive);
    protected void ForceRemovePassive(PassiveEffectDefinition passive) => _passiveEffects.Remove(passive);

    public abstract double GetStat(StatId stat, BattleMember? target = null);

    public bool ApplyEffect(StatusEffectApplicationContext ctx)
    {
        if (ctx.ComputeTotalChance() < ctx.Rng.NextDouble())
        {
            EffectApplyFailure?.Invoke(this, ctx);
            return false;
        }

        EffectApplySuccess?.Invoke(this, ctx);

        var item = _activeEffects.FirstOrDefault(se => se.Definition == ctx.Definition);

        if (item is null)
        {
            _activeEffects.Add(new StatusEffectInstance(ctx));
            foreach (var mutator in ctx.Definition.Mutators)
            {
                mutator.OnApply(ctx);
            }

            return true;
        }

        item.Stacks += ctx.InitialStacks;
        item.Turns = ctx.InitialTurns;

        return true;
    }

    public bool RemoveEffect(StatusEffectRemovalContext ctx)
    {
        var item = _activeEffects.First(ctx.Query.Invoke);
        if (ctx.Rng.NextDouble() < ctx.RemovalChance && _activeEffects.Remove(item))
        {
            EffectRemoveSuccess?.Invoke(this, ctx);
        }

        EffectRemoveFailure?.Invoke(this, ctx);
        return false;
    }
}

public static class BattleMemberExtensions
{
    /// <summary>
    /// Gets the team of which this BattleMember is a part of, or null if it is not part of them.
    /// </summary>
    /// <param name="member">The member whose team to find.</param>
    /// <param name="teams">A list of teams to look through for the allied team.</param>
    /// <returns>The team which contains the given member.</returns>
    /// <exception cref="ArgumentNullException">member or list of teams is null.</exception>
    /// <exception cref="System.InvalidOperationException">The battle member is part of
    /// more than one of the given teams.</exception>
    public static Team? GetAlliedTeam(this BattleMember member, IEnumerable<Team> teams)
    {
        ArgumentNullException.ThrowIfNull(member);
        ArgumentNullException.ThrowIfNull(teams);
        return teams.SingleOrDefault(team => team.Members.Contains(member));
    }
}