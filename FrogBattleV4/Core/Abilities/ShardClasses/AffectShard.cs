using System.Collections.Generic;

namespace FrogBattleV4.Core.Abilities.ShardClasses;

/// <summary>
/// Affect shards do something to the selected targets.
/// They require at least one targeting shard before them.
/// </summary>
public class AffectShard(IEnumerable<ShardComponent> components) : AbilityShard(components)
{
}