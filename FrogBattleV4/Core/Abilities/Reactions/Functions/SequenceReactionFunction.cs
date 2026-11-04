using System;
using System.Collections.Immutable;

namespace FrogBattleV4.Core.Abilities.Reactions.Functions;

public class SequenceReactionFunction : ReactionFunction
{
    public required ImmutableArray<ReactionFunction> Terms { get; init; }
    public int Repeat { get; init; } = 1;

    public override void HandleEvent(EntityUid holder, BattleEnvironment env, EventArgs args)
    {
        for (var i = 0; i < Repeat; i++)
        {
            foreach (var function in Terms)
            {
                function.HandleEvent(holder, env, args);
            }
        }
    }
}