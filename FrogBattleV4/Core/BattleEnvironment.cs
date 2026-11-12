using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Modifiers.StatusEffects;
using FrogBattleV4.Core.Registries;

namespace FrogBattleV4.Core;

public record BattleEnvironment
{
    private readonly ImmutableDictionary<EntityUid, EntityInstance> _entities;
    private readonly ImmutableDictionary<TeamUid, TeamInstance> _teams;
    private readonly Registry<FighterId, Fighter> _fighterRegistry;
    private readonly Registry<ShardId, Shard> _shardRegistry;
    private readonly ImmutableList<IModifierProvider> _arenaModifiers;

    public BattleEnvironment(
        ImmutableDictionary<EntityUid, EntityInstance> entities,
        ImmutableDictionary<TeamUid, TeamInstance> teams,
        Registry<FighterId, Fighter> fighterRegistry,
        Registry<ShardId, Shard> shardRegistry,
        ImmutableList<IModifierProvider> arenaModifiers)
    {
        _entities = entities;
        _teams = teams;
        _fighterRegistry = fighterRegistry;
        _shardRegistry = shardRegistry;
        _arenaModifiers = arenaModifiers;
    }

    /// <summary>
    /// Subjective modifiers are modifiers that should only apply to the current user.
    /// Used to propagate bonus modifiers from abilities and such.
    /// </summary>
    public ImmutableList<IModifierProvider> SubjectiveModifiers { get; private init; } = ImmutableList<IModifierProvider>.Empty;

    #region Methods

    /// <summary>
    /// Adds the specified modifiers to the SubjectiveModifiers list.
    /// </summary>
    /// <param name="modifiers">Modifiers to append.</param>
    /// <returns>A new BattleEnvironment.</returns>
    public BattleEnvironment WithSubjectMods(IEnumerable<IModifierProvider> modifiers)
    {
        return this with
        {
            SubjectiveModifiers = SubjectiveModifiers.AddRange(modifiers),
        };
    }

    /// <summary>
    /// Determines whether an entity fulfills the given predicate in the current environment.
    /// </summary>
    /// <param name="entity">Entity to validate the predicate against.</param>
    /// <param name="predicate">Predicate to check.</param>
    /// <returns>True if <paramref name="predicate"/> matches, false otherwise.</returns>
    public bool IsPredicate(EntityUid entity, EntityPredicate? predicate)
    {
        EnsureExists(entity);
        return predicate.SafeEvaluate(_entities[entity]);
    }

    /// <summary>
    /// Returns the shards that the specified entity can use on a turn.
    /// </summary>
    /// <param name="entity">Entity UID.</param>
    /// <exception cref="InvalidOperationException">
    /// There exists no entity with ID <paramref name="entity"/>
    /// in the current environment.
    /// </exception>
    /// <returns>A collection of every shard this entity has available.</returns>
    public IEnumerable<ShardId> GetShards(EntityUid entity)
    {
        EnsureExists(entity);
        return _fighterRegistry.Get(_entities[entity].Owner).Shards;
    }

    /// <summary>
    /// Returns every member from the same team (on the same "field") as <paramref name="entity"/>.
    /// </summary>
    /// <param name="entity">Entity UID.</param>
    /// <exception cref="InvalidOperationException">
    /// There exists no entity with ID <paramref name="entity"/>
    /// in the current environment.
    /// </exception>
    /// <returns>Every member from the same field as the given entity.</returns>
    public IEnumerable<EntityUid> GetNeighbors(EntityUid entity)
    {
        EnsureExists(entity);
        return _teams[_entities[entity].Team].GetOnFieldMembers().Where(e => !_entities[e].Downed);
    }

    public IEnumerable<EntityUid> GetEnemies(EntityUid entity)
    {
        EnsureExists(entity);
        return _teams.Values
            .SelectMany(team => team.GetOnFieldMembers().Where(e => !_entities[e].Downed))
            .Except(GetNeighbors(entity));
    }

    /// <summary>
    /// Gets the full list of modifiers currently active for the entity with the given UID.
    /// </summary>
    /// <param name="entity">UID of the requested entity.</param>
    /// <returns>A collection containing every modifier relevant to the given entity.</returns>
    /// <exception cref="InvalidOperationException">
    /// There exists no entity with ID <paramref name="entity"/>
    /// in the current environment.
    /// </exception>
    /// <exception cref="InvalidOperationException">DEBUG: ImmutableArray.IsDefault</exception>
    public IEnumerable<IModifierProvider> GetModifiers(EntityUid entity)
    {
        EnsureExists(entity);
        var data = _entities[entity];
        var passiveMods = data.PassiveModifiers;

        return _arenaModifiers
            .Concat(data.ActiveEffects)
            .Concat(passiveMods)
            .Concat(_teams[data.Team].Modifiers);
    }

    /// <summary>
    /// Returns the current value of the given pool for the given entity.
    /// </summary>
    /// <param name="entity">Entity UID.</param>
    /// <param name="pool">Pool ID.</param>
    /// <returns>
    /// A double representing the current value of the pool,
    /// or zero if this entity does not have the specified pool.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// There exists no entity with ID <paramref name="entity"/>
    /// in the current environment.
    /// </exception>
    public double GetPoolValue(EntityUid entity, PoolName pool)
    {
        EnsureExists(entity);
        return _entities[entity].Pools.GetValueOrDefault(pool)?.Value ?? 0;
    }

    /// <summary>
    /// Returns a mutable reference to a counter belonging to the specified entity.
    /// </summary>
    /// <param name="entity">Entity UID.</param>
    /// <param name="counter">Counter ID.</param>
    /// <returns>
    /// A mutable counter that belongs to the given entity,
    /// or null if this entity does not have the specified counter.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// There exists no entity with ID <paramref name="entity"/>
    /// in the current environment.
    /// </exception>
    public Counter? GetCounter(EntityUid entity, CounterName counter)
    {
        EnsureExists(entity);
        return _entities[entity].Counters.GetValueOrDefault(counter);
    }

    public IEnumerable<StatusEffectInstance> GetStatusEffects(EntityUid entity)
    {
        EnsureExists(entity);
        return _entities[entity].ActiveEffects;
    }

    private void EnsureExists(EntityUid entity)
    {
        if (!_entities.ContainsKey(entity)) throw new EntityMissingException(entity);
    }

    #endregion

    #region Subtypes

    /// <summary>
    /// The exception that is thrown when an entity ID does not match any known entity.
    /// </summary>
    /// <param name="entity">The EntityUid that didn't match any entity in the current context.</param>
    public class EntityMissingException(EntityUid entity) : ApplicationException
    {
        private readonly EntityUid _entity = entity;

        public EntityUid Entity => _entity;

        public override string Message => $"Entity {Entity} does not exist in the current battle!";
    }

    /// <summary>
    /// The exception that is thrown when trying to add an entity beyond an accepted limit.
    /// </summary>
    /// <param name="limit">The limit that was exceeded.</param>
    public class EntityLimitException(EntityUid entity, long limit) : ApplicationException
    {
        private readonly EntityUid _entity = entity;
        private readonly long _limit = limit;

        public EntityUid Entity => _entity;

        public long Limit => _limit;

        public override string Message => $"Entity {Entity} exceeded limit of {Limit} entities!";
    }

    #endregion
}