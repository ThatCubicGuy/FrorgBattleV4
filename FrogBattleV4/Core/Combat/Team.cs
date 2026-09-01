using System.Collections.Immutable;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Combat;

public class Team
{
    public Team(Selections.ISelectionProvider playerSelectionProvider,
        params GameEntity[] battleMembers)
    {
        System.ArgumentNullException.ThrowIfNull(playerSelectionProvider);
        System.ArgumentNullException.ThrowIfNull(battleMembers);
        Provider = playerSelectionProvider;
        Members = battleMembers.ToImmutableList();
    }

    public Selections.ISelectionProvider Provider { get; }
    public ImmutableList<GameEntity> Members { get; }
}