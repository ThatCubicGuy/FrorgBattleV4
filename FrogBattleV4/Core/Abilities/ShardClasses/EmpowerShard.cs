using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Abilities.ShardClasses;

/// <summary>
/// Empower shards buff the caster or their team or recover resources.
/// They may be used on their own, with no other shards required.
/// </summary>
public class EmpowerShard : Shard
{
    public required ModifierCollection Modifiers { get; set; }
    public override void Resolve(ref LinkResolutionState state, BattleEnvironment env, LinkResolutionBuilder builder)
    {
        state = state with
        {
            Modifiers = state.Modifiers.With(Modifiers)
        };
    }
}