using System.Collections.ObjectModel;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem;

public struct AbilityExecContext
{
    public ICharacter User;
    public ITargetable MainTarget;
    public ReadOnlyCollection<ITargetable> ValidTargets { get; init; }
    public AbilityDefinition Definition;
    public required System.Random Rng;
}