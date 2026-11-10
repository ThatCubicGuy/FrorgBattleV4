using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Abilities.Reactions;

/// <summary>
/// Base class for reactions that depend on a sender-target contract.
/// </summary>
/// <typeparam name="TEventArgs">
/// Type of the event args handled by this reaction.
/// Must extend <see cref="BattleCommandEventArgs"/>.
/// </typeparam>
public abstract class TargetedReaction<TEventArgs> : Reaction<TEventArgs> where TEventArgs : BattleCommandEventArgs
{
    /// <summary>
    /// The entities that must be a sender of the battle command in order for this reaction to trigger.
    /// Null returns true for all.
    /// </summary>
    public ReactionEntity? Sender { get; init; }
    /// <summary>
    /// The entities that must be a target of the battle command in order for this reaction to trigger.
    /// Null returns true for all.
    /// </summary>
    public ReactionEntity? Target { get; init; }

    public sealed override bool Matches(EntityUid holder, BattleEnvironment env, TEventArgs args)
    {
        return Sender switch
        {
            null => true,
            ReactionEntity.This => holder == args.Sender,
            ReactionEntity.Allies => env.GetNeighbors(holder).Contains(args.Sender),
            ReactionEntity.Enemies => env.GetEnemies(holder).Contains(args.Sender),
            _ => false,
        } && Target switch
        {
            null => true,
            ReactionEntity.This => holder == args.Target,
            ReactionEntity.Allies => env.GetNeighbors(holder).Contains(args.Target),
            ReactionEntity.Enemies => env.GetEnemies(holder).Contains(args.Target),
            _ => false,
        } && MatchesPredicate(holder, env, args);
    }

    [Pure]
    protected abstract bool MatchesPredicate(EntityUid holder, BattleEnvironment env, TEventArgs args);
}