using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Modifiers.StatusEffects;
using FrogBattleV4.Core.Registries;

namespace FrogBattleV4.Core;

public record BattleEnvironment
{
    private readonly FighterRegistry _fighterRegistry;
    private readonly ShardRegistry _shardRegistry;

    public BattleEnvironment(ImmutableDictionary<EntityUid, Entity> entities,
        ImmutableDictionary<TeamUid, Team> teams,
        FighterRegistry fighterRegistry,
        ShardRegistry shardRegistry)
    {
        _fighterRegistry = fighterRegistry;
        _shardRegistry = shardRegistry;
        Entities = entities;
        Teams = teams;
    }

    #region Properties

    private ImmutableDictionary<EntityUid, Entity> Entities { get; }
    private ImmutableDictionary<TeamUid, Team> Teams { get; }
    private ImmutableList<IModifierProvider> ArenaModifiers { get; init; } = ImmutableList<IModifierProvider>.Empty;

    /// <summary>
    /// Subjective modifiers are modifiers that should only apply to the current user.
    /// Used to propagate bonus modifiers from abilities and such.
    /// </summary>
    public ImmutableList<IModifierProvider> SubjectiveModifiers { get; init; } = ImmutableList<IModifierProvider>.Empty;

    #endregion

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
            SubjectiveModifiers = SubjectiveModifiers.AddRange(modifiers)
        };
    }

    /// <summary>
    /// Determines whether an entity fulfills the given predicate in the current environment.
    /// </summary>
    /// <param name="entity">Entity to validate the predicate against.</param>
    /// <param name="predicate">Predicate to check.</param>
    /// <returns>True if <paramref name="predicate"/> matches, false otherwise.</returns>
    public bool IsPredicate(EntityUid entity, EntityPredicate predicate)
    {
        EnsureExists(entity);
        return predicate.Matches(Entities[entity]);
    }

    public IEnumerable<ShardId> GetShards(EntityUid entity)
    {
        EnsureExists(entity);
        return _fighterRegistry.Get(Entities[entity].Owner).Shards;
    }

    /// <summary>
    /// Returns every member from the same field (on the same "line") as <paramref name="entity"/>.
    /// </summary>
    /// <param name="entity">Entity UID.</param>
    /// <returns>Every member from the same field as the given entity.</returns>
    public IEnumerable<EntityUid> GetNeighbors(EntityUid entity)
    {
        EnsureExists(entity);
        return Teams[Entities[entity].Team].GetOnFieldMembers();
    }

    public IEnumerable<EntityUid> GetEnemies(EntityUid entity)
    {
        EnsureExists(entity);
        return Teams.Values
            .SelectMany(team => team.GetOnFieldMembers())
            .Except(Teams[Entities[entity].Team].GetOnFieldMembers());
    }

    /// <summary>
    /// Gets the full list of modifiers currently active for the entity with the given UID.
    /// </summary>
    /// <param name="entity">UID of the requested entity.</param>
    /// <returns>A collection containing every modifier relevant to the given entity.</returns>
    /// <exception cref="InvalidOperationException">
    /// There exists no entity with ID <paramref name="entity"/>
    /// in the current context.
    /// </exception>
    public IEnumerable<IModifierProvider> GetModifiers(EntityUid entity)
    {
        EnsureExists(entity);
        var data = Entities[entity];
        return ArenaModifiers
            .Concat(data.StatusEffects)
            .Concat(_fighterRegistry.Get(data.Owner).PassiveModifiers)
            .Concat(Teams[data.Team].Modifiers);
    }

    /// <summary>
    /// Returns the current value of the given pool for the given entity.
    /// </summary>
    /// <param name="entity">Entity UID.</param>
    /// <param name="pool">Pool ID.</param>
    /// <returns>A double representing the current value of the pool.</returns>
    /// <exception cref="InvalidOperationException">
    /// There exists no entity with ID <paramref name="entity"/>
    /// in the current context.
    /// </exception>
    public double GetPoolValue(EntityUid entity, PoolName pool)
    {
        EnsureExists(entity);
        return Entities[entity].Pools[pool].Value;
    }

    public IEnumerable<StatusEffectInstance> GetStatusEffects(EntityUid entity)
    {
        EnsureExists(entity);
        return Entities[entity].StatusEffects;
    }

    private void EnsureExists(EntityUid entity)
    {
        if (!Entities.ContainsKey(entity)) throw new EntityMissingException(entity);
    }

    #endregion

    #region Subtypes

    /// <summary>
    /// The exception that is thrown when an entity ID does not match any known entity.
    /// </summary>
    /// <param name="entity">The EntityUid that didn't match any entity in the current context.</param>
    public class EntityMissingException(EntityUid entity) : Exception
    {
        public EntityUid Entity { get; } = entity;
        public override string Message => $"Entity {Entity} does not exist in the current battle!";
    }

    /// <summary>
    /// The exception that is thrown when trying to add an entity beyond an accepted limit.
    /// </summary>
    /// <param name="limit">The limit that was exceeded.</param>
    public class EntityLimitException(EntityUid entity, long limit) : Exception
    {
        public EntityUid Entity { get; } = entity;
        public long Limit { get; } = limit;
        public override string Message => $"Entity {Entity} exceeded limit of {Limit} entities!";
    }

    #endregion
}