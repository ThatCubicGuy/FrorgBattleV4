using System.Linq;
using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Abilities.Components.Targeting;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Abilities.ShardClasses;

/// <summary>
/// Affect shards do something to the selected targets.
/// </summary>
public class AffectShard : Shard
{
    public required IShardCommandComponent CommandComponents { get; init; }
    public IShardTargeting Targeting { get; init; } = FilteredTargeting.Identity;
    public ModifierCollection Modifiers { get; init; } = ModifierCollection.Empty;
    public override void Resolve(ref LinkResolutionState state, BattleEnvironment env, CommandBuilder builder)
    {
        var scope = new ShardResolutionScope(state.User, null!, state.Modifiers, /*TODO:*/ builder.Rng);
        CommandComponents.GenerateCommands(state with
        {
            Selections = state.Selections.SelectMany(targeting =>
                Targeting.GenerateInstruction(scope with { Targeting = targeting }, env).Resolve(builder.Rng)),
            Modifiers = Modifiers.With(Modifiers)
        }, env, builder);
    }
}