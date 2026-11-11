using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Reactions.Functions;

/// <summary>
/// A command reaction function can only react to a command event -
/// that is, it can only resolve when given a sender-target relation context.
/// </summary>
public abstract class CommandReactionFunction : ReactionFunction
{
    [JsonPropertyName("entity")]
    public FunctionTarget AffectedEntity { get; init; }

    public override void HandleEvent(EntityUid holder, EventArgs args, BattleEnvironment env, CommandBuilder builder)
    {
        if (args is not BattleCommandEventArgs battleArgs) return;

        HandleEvent(holder, env, battleArgs, builder);
    }

    /// <summary>
    /// Gets the relation contexts relevant to this reaction function based on the AffectedEntity property.
    /// The holder is always the actor.
    /// </summary>
    /// <param name="holder">Entity which launched this reaction function.</param>
    /// <param name="env">Environment to get participants from.</param>
    /// <param name="args">CommandEventArgs for this reaction.</param>
    /// <returns>A collection of relation contexts with the targets of this function.</returns>
    /// <exception cref="NotSupportedException">Unknown FunctionTarget value.</exception>
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

    protected abstract void HandleEvent(EntityUid holder, BattleEnvironment env, BattleCommandEventArgs args,
        CommandBuilder builder);
}