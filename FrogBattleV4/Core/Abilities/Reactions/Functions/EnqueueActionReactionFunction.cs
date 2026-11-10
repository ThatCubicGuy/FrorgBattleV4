using System.Collections.Immutable;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Abilities.Reactions.Functions;

public class EnqueueActionReactionFunction : CommandReactionFunction
{
    public required ImmutableArray<ShardId> Link { get; init; }
    protected override void HandleEvent(EntityUid holder, BattleEnvironment env, BattleCommandEventArgs args,
        CommandBuilder builder)
    {
        var commands = GetRelation(holder, env, args).Select(context => new EnqueueActionCommand
        {
            Data = new EnqueueActionData(new TurnDecision(Link, context.Target)),
            Relation = context with { Target = context.Actor },
        });

        foreach (var command in commands)
        {
            builder.Emit(command);
        }
    }
}