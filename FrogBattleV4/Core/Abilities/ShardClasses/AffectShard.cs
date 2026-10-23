using System.Linq;
using FrogBattleV4.Core.Abilities.Targeting;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Abilities.ShardClasses;

/// <summary>
/// Affect shards do something to the selected targets.
/// </summary>
public class AffectShard : Shard
{
    public required IShardCommandComponent CommandComponents { get; init; }
    public IShardTargeting Targeting { get; init; } = FilterTargeting.Identity;
    public ModifierCollection Modifiers { get; init; } = ModifierCollection.Empty;
    public override void Resolve(ref LinkResolutionState state, BattleEnvironment env, CommandBuilder builder,
        IBattleRng rng)
    {
        var user = state.User;
        var selections = state.Selections.SelectMany(targeting =>
            Targeting.GenerateInstruction(targeting, env).Resolve(rng));
        var mods = state.Modifiers.With(Modifiers);
        foreach (var targeting in selections)
        {
            CommandComponents.GenerateCommands(new ShardResolutionScope(user, targeting, mods), env, builder);
        }
    }
}