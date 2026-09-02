using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Requirements;

/// <summary>
/// Logical OR operator between the given requirements.
/// </summary>
/// <remarks>For fulfillment generation, only the first fulfilled term is executed.</remarks>
public class AnyRequirement : IShardRequirement
{
    public required ImmutableList<IShardRequirement> Terms { get; init; }

    [Pure]
    public virtual bool IsFulfilled(LinkResolutionState state, BattleEnvironment env)
    {
        return Terms.Any(requirement => requirement.IsFulfilled(state, env));
    }

    public void GenerateFulfill(LinkResolutionState state, BattleEnvironment env, LinkResolutionBuilder builder)
    {
        Terms.First(requirement => requirement.IsFulfilled(state, env)).GenerateFulfill(state, env, builder);
    }
}