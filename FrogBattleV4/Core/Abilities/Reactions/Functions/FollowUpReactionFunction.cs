using System.Collections.Immutable;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Abilities.Reactions.Functions;

public class FollowUpReactionFunction : CommandReactionFunction
{
    public required ImmutableArray<ShardId> Shards { get; init; }
    protected override void HandleEvent(EntityUid holder, BattleEnvironment env, BattleCommandEventArgs args,
        CommandBuilder builder)
    {
        var result = GetRelation(holder, env, args).Select(target =>
            new TurnDecision(Shards, target.Target));

        foreach (var decision in result)
        {
            builder.Emit(new EnqueueActionCommand
            {
                Data = new EnqueueActionData(decision, ActionType.FollowUp),
                // Enqueue an instant action for ourselves
                Relation = new RelationContext
                {
                    Actor = holder,
                    Target = holder,
                },
            });
        }
    }
}