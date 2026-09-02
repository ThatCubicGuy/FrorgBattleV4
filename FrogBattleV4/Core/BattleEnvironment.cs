using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core;

public record BattleEnvironment
{
    #region Properties

    private static long MaxPlayersPerTeam => 4;
    private ImmutableDictionary<EntityUid, TeamUid> MemberAffiliations { get; }
    private ImmutableDictionary<TeamUid, ImmutableList<EntityUid>> TeamMembers { get; }
    private ImmutableDictionary<EntityUid, BattleMemberData> MemberData { get; }
    private ImmutableDictionary<TeamUid, Team> Teams { get; }
    private Random Rng { get; }

    public BattleEnvironment(IEnumerable<Team> teams, IEnumerable<BattleMemberData> memberData,
        IEnumerable<KeyValuePair<EntityUid, TeamUid>> relations, Random random)
    {
        Teams = teams.ToImmutableDictionary(team => team.Id);
        MemberData = memberData.ToImmutableDictionary(data => data.Member.Id);
        Rng = random;
        MemberAffiliations = relations.ToImmutableDictionary();
        // smth sth horrible optimization. yes i'll fix it
        var teamMembers = new Dictionary<TeamUid, EntityUid[]>();
        teamMembers.EnsureCapacity(Teams.Count);
        foreach (var kvp in MemberAffiliations)
        {
            if (!teamMembers.TryGetValue(kvp.Value, out var team))
                team = teamMembers[kvp.Value] = new EntityUid[MaxPlayersPerTeam];
            else if (team.Length == MaxPlayersPerTeam)
                throw new EntityLimitException(kvp.Key, MaxPlayersPerTeam);
            team[^1] = kvp.Key;
        }
        TeamMembers = teamMembers.ToImmutableDictionary(kvp => kvp.Key, kvp => kvp.Value.ToImmutableList());
    }

    public ModifierCollection ArenaModifiers { get; init; } = ModifierCollection.Empty;

    #endregion

    #region Methods

    /// <summary>
    /// Returns a random number provided by Rng.
    /// </summary>
    /// <returns>
    /// A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.
    /// </returns>
    public double NextRoll()
    {
        return Rng.NextDouble();
    }

    /// <summary>
    /// Returns the allied team of the given entity.
    /// </summary>
    /// <param name="entity">Entity whose team to find.</param>
    /// <returns>The allied team of this entity.</returns>
    /// <exception cref="InvalidOperationException">
    /// There exists no entity with ID <paramref name="entity"/>
    /// in the current context.
    /// </exception>
    public Team GetAlliedTeam(EntityUid entity)
    {
        EnsureExists(entity);
        return Teams[MemberAffiliations[entity]];
    }

    /// <summary>
    /// Returns every member from the same field (on the same "line") as <paramref name="entity"/>
    /// </summary>
    /// <param name="entity">Entity UID.</param>
    /// <returns>Every member from the same field as the given entity.</returns>
    public ImmutableList<EntityUid> GetNeighbors(EntityUid entity)
    {
        EnsureExists(entity);
        return TeamMembers[MemberAffiliations[entity]];
    }

    /// <summary>
    /// Gets the entity with this ID in the current battle.
    /// </summary>
    /// <param name="uid">ID to look up.</param>
    /// <returns>The battle member with this ID.</returns>
    /// <exception cref="InvalidOperationException">
    /// There exists no entity with ID <paramref name="uid"/>
    /// in the current context.
    /// </exception>
    public BattleMemberData GetEntity(EntityUid uid)
    {
        EnsureExists(uid);
        return MemberData[uid];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">
    /// There exists no entity with ID <paramref name="uid"/>
    /// in the current context. -or- The entity is not a fighter.</exception>
    public FighterBase GetFighter(EntityUid uid)
    {
        EnsureExists(uid);
        var member = MemberData[uid].Member;
        return member as FighterBase
               ?? throw new InvalidOperationException($"Entity {uid} is not a fighter!");
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
        return ArenaModifiers
            .With(GetEntity(entity).ProvideModifiers(this))
            .With(GetAlliedTeam(entity).Modifiers);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="pool"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">
    /// There exists no entity with ID <paramref name="entity"/>
    /// in the current context.
    /// </exception>
    public double GetPoolValue(EntityUid entity, PoolId pool)
    {
        EnsureExists(entity);
        return MemberData[entity].PoolValues[pool];
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
    /// <param name="entity"></param>
    public class EntityMissingException(EntityUid entity) : Exception
    {
        public EntityUid Entity { get; } = entity;
        public override string Message => $"Entity {Entity} does not exist in the current battle!";
    }

    /// <summary>
    /// The exception that is thrown when trying to add an entity
    /// </summary>
    /// <param name="limit"></param>
    public class EntityLimitException(EntityUid entity, long limit) : Exception
    {
        public EntityUid Entity { get; } = entity;
        public long Limit { get; } = limit;
        public override string Message => $"Entity {Entity} exceeded limit of {Limit} entities!";
    }

    #endregion
}