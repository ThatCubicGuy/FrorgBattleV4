using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Abilities.Components;

public interface IShardTargeting
{
    [Pure]
    IEnumerable<AbilityTargetingContext> SelectTargets(ShardLinkContext ctx);
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