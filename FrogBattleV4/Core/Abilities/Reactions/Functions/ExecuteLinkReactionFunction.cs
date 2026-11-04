using System.Collections.Immutable;
using System.Linq;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Reactions.Functions;

public class ExecuteLinkReactionFunction : CommandReactionFunction
{
    public required ImmutableArray<ShardId> Shards { get; init; }
    protected override void HandleEvent(EntityUid holder, BattleEnvironment env, BattleCommandEventArgs args)
    {
        // TODO (Nova)
        var result = GetRelation(holder, env, args).Select(target =>
            new Selections.PlayerTurnAction(Shards, target.Target));
    }
}