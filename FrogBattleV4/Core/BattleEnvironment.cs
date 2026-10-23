using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core;

public record BattleEnvironment
{
    public BattleEnvironment(ImmutableDictionary<EntityUid, Entity> entities,
        ImmutableDictionary<TeamUid, Team> teams)
    {
        Entities = entities;
        Teams = teams;
    }

    #region Properties

    private ImmutableDictionary<EntityUid, Entity> Entities { get; }
    private ImmutableDictionary<TeamUid, Team> Teams { get; }

    public ModifierCollection ArenaModifiers { get; init; } = ModifierCollection.Empty;

    #endregion

    #region Methods

    /// <summary>
    /// Determines whether an entity fulfills the given predicate in the current environment.
    /// </summary>
    /// <param name="entity">Entity to validate the predicate against.</param>
    /// <param name="predicate">Predicate to check.</param>
    /// <returns></returns>
    public bool IsPredicate(EntityUid entity, EntityPredicate predicate)
    {
        EnsureExists(entity);
        return predicate.Matches(Entities[entity]);
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

    /// <summary>
    /// Gets the full list of modifiers currently active for the entity with the given UID.
    /// </summary>
    /// <param name="entity">UID of the requested entity.</param>
    /// <returns>A collection containing every modifier relevant to the given entity.</returns>
    /// <exception cref="InvalidOperationException">
    /// There exists no entity with ID <paramref name="entity"/>
    /// in the current context.
    /// </exception>
    public ModifierCollection GetModifiers(EntityUid entity)
    {
        EnsureExists(entity);
        var data = Entities[entity];
        return ArenaModifiers
            .With(data.GetAllModifiers())
            .With(Teams[data.Team].Modifiers);
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