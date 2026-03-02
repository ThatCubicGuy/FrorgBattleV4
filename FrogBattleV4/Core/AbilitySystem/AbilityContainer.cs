#nullable enable
using System.Collections;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.AbilitySystem;

public class AbilityContainer(IEnumerable<AbilityDefinition> definitions) : IEnumerable<AbilityDefinition>, IBattleMemberComponent
{
    private readonly FrozenSet<AbilityDefinition> _abilityDefinitions = definitions.ToFrozenSet();

    public AbilityDefinition? this[string name] => _abilityDefinitions.SingleOrDefault(ad => ad.Name.Equals(name));

    public IEnumerator<AbilityDefinition> GetEnumerator() => _abilityDefinitions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}