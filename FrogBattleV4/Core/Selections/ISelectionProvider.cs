using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core.Abilities;

namespace FrogBattleV4.Core.Selections;

/// <summary>
/// Represents an object that can resolve a selection request for any type of request.
/// </summary>
public interface ISelectionProvider
{
    Task<PlayerTurnAction> GetSelectionAsync(PlayerActionRequest request, CancellationToken cancellationToken);
}

public sealed record PlayerTurnAction(ImmutableList<ShardId> Link, EntityUid MainTarget);

public sealed record PlayerActionRequest(EntityUid User, BattleEnvironment Environment);