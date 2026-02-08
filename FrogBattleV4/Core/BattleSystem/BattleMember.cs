using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.BattleSystem.Actions;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Modifiers;
using FrogBattleV4.Core.EffectSystem.StatusEffects;

namespace FrogBattleV4.Core.BattleSystem;

public abstract class BattleMember : ITakesTurns, IHasStats, IHasPools, ISupportsEffects
{
    private readonly List<StatusEffectInstance> _activeEffects = [];
    private readonly Dictionary<PoolId, PoolComponent> _pools = new();

    public event EventHandler<StatusEffectApplicationContext> EffectApplySuccess;
    public event EventHandler<StatusEffectApplicationContext> EffectApplyFailure;
    public event EventHandler<StatusEffectRemovalContext> EffectRemoveSuccess;
    public event EventHandler<StatusEffectRemovalContext> EffectRemoveFailure;

    [NotNull] public string Name { get; protected init; }
    [NotNull] public IEnumerable<IAction> Turns { get; protected init; } = [];
    [NotNull] public IEnumerable<IDamageable> Parts { get; protected init; } = [];

    [NotNull]
    public IReadOnlyDictionary<PoolId, PoolComponent> Pools
    {
        get => _pools;
        protected init => _pools = value.ToDictionary();
    }
    public bool AddPool(PoolComponent pool) => _pools.TryAdd(pool.Id, pool);
    public bool RemovePool(string poolId) => _pools.Remove(poolId);

    [NotNull] public IEnumerable<IModifierComponent> AttachedEffects { get; } = [];

    public double GetStat(string stat, BattleMember target = null)
    {
        throw new NotImplementedException();
    }

    public bool ApplyEffect(StatusEffectApplicationContext ctx)
    {
        throw new NotImplementedException();
    }

    public bool RemoveEffect(StatusEffectRemovalContext ctx)
    {
        throw new NotImplementedException();
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