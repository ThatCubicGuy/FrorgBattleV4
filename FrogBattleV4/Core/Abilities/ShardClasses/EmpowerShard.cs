using System.Collections.Generic;

namespace FrogBattleV4.Core.Abilities.ShardClasses;

/// <summary>
/// Empower shards buff the caster or their team or recover resources.
/// They may be used on their own, with no other shards required.
/// </summary>
public class EmpowerShard(IEnumerable<ShardComponent> components) : AbilityShard(components)
{
}