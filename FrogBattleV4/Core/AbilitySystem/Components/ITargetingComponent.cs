using System.Collections.Generic;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components;

public interface ITargetingComponent
{
    IEnumerable<TargetingContext> SelectTargets(AbilityExecContext ctx);
}
/// <summary>
/// Applied by the ability at the start to actually generate the list of possible targets,
/// alongside TargetingType (which is for cleaner UI display).
/// </summary>
public interface ITargetFilter
{
    bool Filter(ITargetable target);
}

public enum TargetingType
{
    None,
    Allies,
    Enemies,
    Self,
    Both,
    Arena
}