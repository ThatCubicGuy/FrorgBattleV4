using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Abilities.ShardClasses;

/// <summary>
/// Empower shards buff the current shard link in various ways.
/// </summary>
public class EmpowerShard : Shard
{
    public required IModifierRule[] Modifiers { get; init; }

    public override void Resolve(ref LinkResolutionState state, BattleEnvironment env, CommandBuilder builder,
        IBattleRng rng)
    {
        state = state with
        {
            Modifiers = state.Modifiers.With(Modifiers)
        };
    }
}