using FrogBattleV4.Core.Abilities.Components;

namespace FrogBattleV4.Core.Abilities.ShardClasses;

/// <summary>
/// Target shards are required in most links.
/// They provide targeting, or change it if they are added after another target shard.
/// The main target will always stay the same.
/// Some targeting shards also do something beyond 
/// </summary>
public class TargetShard : Shard
{
    public required IShardTargeting Targeting { get; init; }
    public override void Resolve(ref LinkResolutionState state, BattleEnvironment env, LinkResolutionBuilder builder)
    {
        state = state with
        {
            Selections = Targeting.SelectTargets(state, env)
        };
    }
}