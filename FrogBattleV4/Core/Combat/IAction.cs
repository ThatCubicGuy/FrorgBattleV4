using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core.Decisions;

namespace FrogBattleV4.Core.Combat;

public interface IAction
{
    EntityUid Actor { get; }
    Task<TurnDecision> GetDecisionAsync(BattleEnvironment env, CancellationToken token);
}

/// <summary>
/// An instant action does not have an action value, has a predetermined decision, and is
/// executed before any other turn (scheduled action) and after instant actions enqueued before it.
/// </summary>
/// <param name="Actor">Entity UID of the actor in this action.</param>
/// <param name="Decision">Predefined decision that will be executed when the action happens.</param>
public record InstantAction(EntityUid Actor, TurnDecision Decision) : IAction
{
    public Task<TurnDecision> GetDecisionAsync(BattleEnvironment env, CancellationToken token)
    {
        return Task.FromResult(Decision);
    }
}

/// <summary>
/// A turn is an action scheduled in the battle manager's turn queue with a base action value.
/// </summary>
/// <param name="Actor">The actor that will take action during this turn.</param>
/// <param name="BaseActionValue">The base action value of this turn.</param>
public record ScheduledTurn(EntityUid Actor, double BaseActionValue, IDecisionProvider DecisionProvider) : IAction
{
    public Task<TurnDecision> GetDecisionAsync(BattleEnvironment env, CancellationToken token)
    {
        return DecisionProvider.GetSelectionAsync(new PlayerDecisionRequest(Actor, env), token);
    }
}