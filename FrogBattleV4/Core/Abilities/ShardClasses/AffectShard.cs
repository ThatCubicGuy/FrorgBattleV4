using System.Collections.Immutable;
using System.Linq;
using FrogBattleV4.Core.Abilities.Targeting;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Abilities.ShardClasses;

/// <summary>
/// Affect shards do something to the selected targets.
/// </summary>
public class AffectShard : Shard
{
    public required ImmutableArray<IShardCommandComponent> Components { get; init; }
    public IShardTargeting Targeting { get; init; } = FilterTargeting.Identity;
    public ImmutableArray<IModifierRule> Modifiers { get; init; } = [];
    public override void Resolve(ref LinkResolutionState state, BattleEnvironment env, CommandBuilder builder,
        IBattleRng rng)
    {
        var user = state.User;
        var selections = state.Selections.SelectMany(targeting =>
            Targeting.GenerateInstruction(targeting, env).Resolve(rng));

        var mods = state.Modifiers.Concat(Modifiers);
        var moddedEnv = env.WithSubjectMods(mods);

        foreach (var targeting in selections)
        {
            foreach (var commandComponent in Components)
            {
                commandComponent.GenerateCommands(new ShardResolutionScope(user, targeting), moddedEnv, builder);
            }
        }
    }
}