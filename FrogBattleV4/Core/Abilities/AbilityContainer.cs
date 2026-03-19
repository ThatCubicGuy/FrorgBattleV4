#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Abilities;

public class AbilityContainer(IEnumerable<AbilityDefinition> definitions) : IEnumerable<AbilityDefinition>
{
    private readonly List<AbilityDefinition> _abilityDefinitions = definitions.ToList();

    public AbilityDefinition? this[string name] => _abilityDefinitions.SingleOrDefault(ad => ad.Name.Equals(name));

    public IEnumerator<AbilityDefinition> GetEnumerator() => _abilityDefinitions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}