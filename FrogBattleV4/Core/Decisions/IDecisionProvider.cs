using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core.Combat;

namespace FrogBattleV4.Core.Decisions;

/// <summary>
/// Represents an object that can resolve a player decision request.
/// </summary>
public interface IDecisionProvider
{
    Task<TurnDecision> GetSelectionAsync(PlayerDecisionRequest request, CancellationToken cancellationToken);
}

public sealed record PlayerDecisionRequest(EntityUid User, BattleEnvironment Environment);