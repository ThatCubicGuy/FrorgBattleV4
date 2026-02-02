using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace FrogBattleV4.Core.AbilitySystem.Components.Requirements;

/// <summary>
/// Logical OR operator between the given requirements.
/// </summary>
/// <param name="RequirementList">List of requirements for the operator.</param>
public record AnyRequirement(
    [NotNull] params IRequirementComponent[] RequirementList) : IRequirementComponent
{
    [Pure]
    public bool IsFulfilled(AbilityExecContext ctx)
    {
        return RequirementList.Any(x => x.IsFulfilled(ctx));
    }
}