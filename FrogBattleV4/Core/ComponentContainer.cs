using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core;

public class ComponentContainer : IEnumerable<IBattleMemberComponent>
{
    [NotNull] private readonly List<IBattleMemberComponent> _components = [];

    public event System.Action<IBattleMemberComponent> ComponentAdded;

    public void Add(IBattleMemberComponent component)
    {
        _components.Add(component);
        ComponentAdded?.Invoke(component);
    }

    public IEnumerator<IBattleMemberComponent> GetEnumerator() => _components.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}