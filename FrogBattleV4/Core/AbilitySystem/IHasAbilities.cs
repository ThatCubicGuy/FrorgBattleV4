using System.Collections.Generic;

namespace FrogBattleV4.Core.AbilitySystem;

public interface IHasAbilities
{
    List<AbilityDefinition> Abilities { get; }
}