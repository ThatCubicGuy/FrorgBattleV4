using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Selections;

namespace FrogBattleV4.Core.Combat;

public class Team : UniquelyIdentifiable<TeamUid>
{
    private readonly List<EntityUid> _members = [];
    private readonly List<EntityUid> _offFieldMembers = [];
    public Team(ISelectionProvider playerSelectionProvider, IEnumerable<IModifierProvider>? modifiers = null)
    {
        ArgumentNullException.ThrowIfNull(playerSelectionProvider);
        Provider = playerSelectionProvider;
        Modifiers = modifiers?.ToImmutableList() ?? ImmutableList<IModifierProvider>.Empty;
    }

    public ISelectionProvider Provider { get; }
    public ImmutableList<IModifierProvider> Modifiers { get; private set; }

    public IEnumerable<EntityUid> GetOnFieldMembers() => _members;
    public IEnumerable<EntityUid> GetAllMembers() => _members.Concat(_offFieldMembers);

    public void AddMember(EntityUid entity, EntityUid? nextTo = null)
    {
        if (_members.Contains(entity))
            throw new InvalidOperationException("Cannot add an already added member.");

        if (nextTo.HasValue)
        {
            var idx = _members.IndexOf(nextTo.Value);
            if (idx < 0)
                throw new InvalidOperationException("Cannot add next to a nonexistent member.");
            _members.Insert(idx, entity);
        }
        else _members.Add(entity);
    }

    public void RemoveMember(EntityUid entity)
    {
        if (!_members.Contains(entity))
            throw new InvalidOperationException("Cannot remove a nonexistent member.");

        _members.Remove(entity);
    }
}