using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Abilities.Components;

public interface IShardRequirement
{
    /// <summary>
    /// Determines whether the requirement is fulfilled in this context.
    /// </summary>
    /// <param name="ctx">Context in which to check fulfillment.</param>
    /// <returns>True if the request is satisfied, false otherwise.</returns>
    [Pure]
    bool IsFulfilled(ShardLinkContext ctx);
}