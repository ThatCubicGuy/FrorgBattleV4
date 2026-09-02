using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Requirements;

/// <summary>
/// Logical AND operator between the given requirements.
/// </summary>
public class AllRequirement : IShardRequirement
{
    public required ImmutableList<IShardRequirement> Terms { get; init; }

    [Pure]
    public virtual bool IsFulfilled(LinkResolutionState state, BattleEnvironment env)
    {
        return Terms.All(requirement => requirement.IsFulfilled(state, env));
    }

    public void GenerateFulfill(LinkResolutionState state, BattleEnvironment env, LinkResolutionBuilder builder)
    {
        foreach (var requirement in Terms)
        {
            requirement.GenerateFulfill(state, env, builder);
        }
    }
}