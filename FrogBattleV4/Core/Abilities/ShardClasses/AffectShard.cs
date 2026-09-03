using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Abilities.Components.Targeting;
using FrogBattleV4.Core.Modifiers;

namespace FrogBattleV4.Core.Abilities.ShardClasses;

/// <summary>
/// Affect shards do something to the selected targets.
/// </summary>
public class AffectShard : Shard
{
    public required IShardCommand Commands { get; init; }
    public IShardTargeting Targeting { get; init; } = FilteredTargeting.Identity;
    public ModifierCollection Modifiers { get; init; } = ModifierCollection.Empty;
    public override void Resolve(ref LinkResolutionState state, BattleEnvironment env, LinkResolutionBuilder builder)
    {
        Commands.Generate(state with
        {
            Selections = Targeting.SelectTargets(state, env),
            Modifiers = Modifiers.With(Modifiers)
        }, env, builder);
    }
}