using System;
using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.Abilities.Reactions.Functions;

namespace FrogBattleV4.Core.Abilities.Reactions;

/// <summary>
/// Represents a reaction that will handle an event which does not provide event data.
/// </summary>
public abstract class Reaction : IReaction
{
    public required ImmutableArray<ReactionFunction> Functions { get; init; }

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

    [Pure]
    public abstract bool Matches(EntityUid holder, BattleEnvironment env, TEventArgs args);

    public abstract void Accept(IReactionVisitor visitor);
}