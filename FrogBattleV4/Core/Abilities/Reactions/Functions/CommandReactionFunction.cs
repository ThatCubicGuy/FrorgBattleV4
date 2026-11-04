using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Reactions.Functions;

public abstract class CommandReactionFunction : ReactionFunction
{
    [JsonPropertyName("entity")]
    public FunctionTarget AffectedEntity { get; init; }

    public override void HandleEvent(EntityUid holder, BattleEnvironment env, EventArgs args)
    {
        if (args is not BattleCommandEventArgs battleArgs) return;
        
        HandleEvent(holder, env, battleArgs);
    }

    protected IEnumerable<RelationContext> GetRelation(EntityUid holder, BattleEnvironment env,
        IBattleCommandEventArgs args)
    {
        return (AffectedEntity switch
        {
            FunctionTarget.This => [holder],
            FunctionTarget.Sender => [args.Sender],
            FunctionTarget.Target => [args.Target],
            FunctionTarget.Allies => env.GetNeighbors(holder),
            FunctionTarget.Enemies => env.GetNeighbors(args.Target),
            _ => throw new NotSupportedException("Unknown FunctionTarget: " + AffectedEntity),
        }).Select(target => new RelationContext
        {
            Actor = holder,
            Target = target,
        });
    }

    protected abstract void HandleEvent(EntityUid holder, BattleEnvironment env, BattleCommandEventArgs args);
}