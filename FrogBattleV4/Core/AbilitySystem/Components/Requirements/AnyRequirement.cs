using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;

namespace FrogBattleV4.Core.AbilitySystem.Components.Requirements;

/// <summary>
/// Logical OR operator between the given requirements.
/// </summary>
public class AnyRequirement : IAbilityRequirementComponent
{
    private readonly List<IAbilityRequirementComponent> _requirements;

    [NotNull]
    public required IEnumerable<IAbilityRequirementComponent> Requirements
    {
        get => _requirements;
        init => _requirements = value.ToList();
    }

    [Pure]
    public bool IsFulfilled(AbilityExecContext ctx)
    {
        return Requirements.Any(rc => rc.IsFulfilled(ctx));
    }
}