using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Selections;

namespace FrogBattleV4.Core.Combat;

public class Team : Identifiable<TeamUid>
{
    private readonly List<EntityUid> _members = [];

    public Team(ISelectionProvider playerSelectionProvider, IEnumerable<IModifierProvider>? modifiers = null)
    {
        ArgumentNullException.ThrowIfNull(playerSelectionProvider);
        Provider = playerSelectionProvider;
        Members = new ReadOnlyCollection<EntityUid>(_members);
        Modifiers = modifiers is not null ? new ModifierCollection([.. modifiers]) : ModifierCollection.Empty;
    }

    public ISelectionProvider Provider { get; }
    public ReadOnlyCollection<EntityUid> Members { get; }
    public ModifierCollection Modifiers { get; }

    public IEnumerable<EntityUid> GetOnFieldMembers() => _members;

    public void AddMember(EntityUid entity)
    {
        if (_members.Contains(entity))
            throw new InvalidOperationException("Cannot add an already added member");

        _members.Add(entity);
    }

    public void RemoveMember(EntityUid entity)
    {
        if (!_members.Contains(entity))
            throw new InvalidOperationException("Cannot remove a nonexistent member");

        _members.Remove(entity);
    }
}