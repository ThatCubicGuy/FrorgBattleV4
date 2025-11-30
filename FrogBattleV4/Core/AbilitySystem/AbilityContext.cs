using System.Collections.Generic;
using System.Collections.ObjectModel;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem;

public struct AbilityContext
{
    public ICharacter User;
    public ReadOnlyCollection<ITargetable> ValidTargets;
    public AbilityDefinition Definition;
    public System.Random Rng;
}