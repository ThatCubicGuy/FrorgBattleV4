using System.Collections.Generic;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem;

public struct AbilityContext
{
    public ICharacter User;
    public IReadOnlyList<ITargetable> ValidTargets;
    public AbilityDefinition Definition;
    public System.Random Rng;
}