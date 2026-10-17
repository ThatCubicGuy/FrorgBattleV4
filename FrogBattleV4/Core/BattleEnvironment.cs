using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core;

public class BattleEnvironment(
    ImmutableDictionary<EntityUid, FighterData> memberData,
    ImmutableDictionary<TeamUid, Team> teams,
    Random random)
{
    #region Properties

    private ImmutableDictionary<EntityUid, FighterData> MemberData { get; } = memberData;
    private ImmutableDictionary<TeamUid, Team> Teams { get; } = teams;

    public ModifierCollection ArenaModifiers { get; init; } = ModifierCollection.Empty;

    #endregion

    #region Methods

    /// <summary>
    /// Returns a random number provided by Rng.
    /// </summary>
    /// <returns>
    /// A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.
    /// </returns>
    internal double NextRoll()
    {
        return random.NextDouble();
    }

    /// <summary>
    /// Returns every member from the same field (on the same "line") as <paramref name="entity"/>.
    /// </summary>
    /// <param name="entity">Entity UID.</param>
    /// <returns>Every member from the same field as the given entity.</returns>
    public IEnumerable<EntityUid> GetNeighbors(EntityUid entity)
    {
        EnsureExists(entity);
        return Teams[MemberData[entity].Team].GetOnFieldMembers();
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
        var data = MemberData[entity];
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
    public double GetPoolValue(EntityUid entity, PoolId pool)
    {
        EnsureExists(entity);
        return MemberData[entity].Pools[pool].Value;
    }

    public IEnumerable<StatusEffectInstance> GetStatusEffects(EntityUid entity)
    {
        EnsureExists(entity);
        return MemberData[entity].StatusEffects;
    }

    private void EnsureExists(EntityUid entity)
    {
        if (!MemberData.ContainsKey(entity)) throw new EntityMissingException(entity);
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