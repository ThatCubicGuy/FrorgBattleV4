using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core;

// Metadata
public partial record BattleContext
{
    public BattleContext(IEnumerable<BattleMember> members)
    {
        Members = [.. members];
    }

    public required ImmutableList<BattleMember> Members { get; init; }
    public required Team TeamA { get; init; }
    public required Team TeamB { get; init; }
    public ImmutableList<IModifierProvider> ArenaModifiers { get; init; } = ImmutableList<IModifierProvider>.Empty;
}

// Methods
public partial record BattleContext
{
    
    /// <summary>
    /// Returns the allied team of the given entity.
    /// </summary>
    /// <param name="entity">Entity whose team to find.</param>
    /// <returns>The allied team of this entity.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="entity"/> is not part of any team.</exception>
    public Team AlliedTeamOf(EntityId entity)
    {
        if (TeamA.Members.Contains(entity)) return TeamA;
        if (TeamB.Members.Contains(entity)) return TeamB;

        throw new InvalidOperationException($"Entity {entity} is not part of any team!");
    }
    
    /// <summary>
    /// Returns the enemy team of the given entity.
    /// </summary>
    /// <param name="entity">Entity whose opposing team to find.</param>
    /// <returns>The enemy team of this entity.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="entity"/> is not part of any team.</exception>
    public Team EnemyTeamOf(EntityId entity)
    {
        if (TeamA.Members.Contains(entity)) return TeamB;
        if (TeamB.Members.Contains(entity)) return TeamA;

        throw new InvalidOperationException($"Entity {entity} is not part of any team!");
    }

    public BattleMember GetEntity(EntityId id)
    {
        return Members.SingleOrDefault(x => x.Id == id)
               ?? throw new InvalidOperationException($"Entity {id} is not part of any team!");
    }
    public FighterBase GetFighter(EntityId id)
    {
        var member = Members.SingleOrDefault(x => x.Id == id)
                     ?? throw new InvalidOperationException($"Entity {id} is not part of any team!");
        return member is FighterBase fighter
            ? fighter
            : throw new InvalidOperationException($"Entity {id} is not a fighter!");
    }

    public IModifierProvider GetModifiersFor(EntityId id)
    {
        var result = new ModifierCollection(ArenaModifiers);
    }
}