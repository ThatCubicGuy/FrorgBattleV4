using System.Linq;
using FrogBattleV4.Core.Abilities.Components;

namespace FrogBattleV4.Core.Abilities.ShardClasses;

/// <summary>
/// Target shards are required in most links.
/// They provide targeting, or change it if they are added after another target shard.
/// </summary>
public class TargetShard : Shard
{
    public required IShardTargeting Targeting { get; init; }
    public override void Resolve(ref LinkResolutionState state, BattleEnvironment env, CommandBuilder builder,
        IBattleRng rng)
    {
        state = state with
        {
            Selections = state.Selections.SelectMany(targeting =>
                // TODO (Nova): Somehow get rid of RNG here as well?
                Targeting.GenerateInstruction(targeting, env).Resolve(rng))
        };
    }
}