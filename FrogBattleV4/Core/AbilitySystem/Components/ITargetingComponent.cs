using System.Collections.Generic;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components;

public interface ITargetingComponent
{
    IEnumerable<AbilityTargetingContext> SelectTargets(AbilityExecContext ctx);
}
/// <summary>
/// Applied by the ability at the start to actually generate the list of possible targets.
/// </summary>
public interface ITargetFilter
{
    bool Filter(ITargetable target);
}

public enum TargetingPool
{
    None,
    Allies,
    Enemies,
    Self,
    Both,
    Arena
}