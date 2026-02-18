using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace FrogBattleV4.Core.AbilitySystem.Components.Requirements;

/// <summary>
/// Logical OR operator between the given requirements.
/// </summary>
/// <param name="RequirementArray">List of requirements for the operator.</param>
public record AnyRequirement([NotNull] params IRequirementComponent[] RequirementArray) : IRequirementComponent
{
    [Pure]
    public bool IsFulfilled(AbilityExecContext ctx)
    {
        return RequirementArray.Any(rc => rc.IsFulfilled(ctx));
    }
}