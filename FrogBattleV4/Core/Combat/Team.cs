using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Combat;

public class Team
{
    public Team([NotNull] Selections.ISelectionProvider playerSelectionProvider,
        [NotNull] params IBattleMember[] battleMembers)
    {
        System.ArgumentNullException.ThrowIfNull(playerSelectionProvider);
        System.ArgumentNullException.ThrowIfNull(battleMembers);
        Provider = playerSelectionProvider;
        Members = battleMembers.ToImmutableList();
    }

    [NotNull] public Selections.ISelectionProvider Provider { get; }
    [NotNull] public ImmutableList<IBattleMember> Members { get; }
}