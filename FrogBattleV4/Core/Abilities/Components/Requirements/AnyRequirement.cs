using System.Collections.Immutable;
using System.Diagnostics.Contracts;
using System.Linq;

namespace FrogBattleV4.Core.Abilities.Components.Requirements;

/// <summary>
/// Logical OR operator between the given requirements.
/// </summary>
public class AnyRequirement : IShardRequirement
{
    public required ImmutableList<IShardRequirement> Terms { get; init; }

    [Pure]
    public virtual bool IsFulfilled(ShardLinkContext ctx)
    {
        return Terms.Any(requirement => requirement.IsFulfilled(ctx));
    }
}