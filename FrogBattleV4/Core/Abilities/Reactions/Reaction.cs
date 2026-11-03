using System;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Abilities.Reactions;

/// <summary>
/// Represents a reaction that will handle an event which does not provide event data.
/// </summary>
public abstract class Reaction : IReaction
{
    public required ImmutableArray<ReactionFunction> Functions { get; init; }

    public void HandleEvent(EntityUid holder, BattleEnvironment env, EventArgs args)
    {
        if (!Matches(holder, env)) return;
        foreach (var function in Functions)
        {
            function.HandleEvent(holder, env, args);
        }
    }

    protected abstract bool Matches(EntityUid holder, BattleEnvironment env);

    public abstract void Accept(IReactionVisitor visitor);
}

/// <summary>
/// Represents a reaction that will handle an event which provides event data.
/// </summary>
/// <typeparam name="TEventArgs">Type of the event data this reaction can handle.</typeparam>
public abstract class Reaction<TEventArgs> : IReaction where TEventArgs : EventArgs
{
    // TODO (Nova)
    public required ImmutableArray<ReactionFunction> Functions { get; init; }

    public void HandleEvent(EntityUid holder, BattleEnvironment env, TEventArgs args)
    {
        if (!Matches(holder, env, args)) return;
        foreach (var function in Functions)
        {
            function.HandleEvent(holder, env, args);
        }
    }

    [Pure]
    protected abstract bool Matches(EntityUid holder, BattleEnvironment env, TEventArgs args);

    public abstract void Accept(IReactionVisitor visitor);
}