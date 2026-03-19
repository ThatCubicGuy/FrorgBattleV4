using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Abilities.Components;

namespace FrogBattleV4.Core.Calculation;

internal static class AbilityPipeline
{
    [Pure]
    public static IEnumerable<IBattleCommand> GetCommands(this AbilityExecContext ctx)
    {
        return ctx.Definition.Components
            .OfType<IAbilityCommandComponent>()
            .SelectMany(acc => acc.GetContribution(ctx));
    }

    [Pure]
    public static AbilityPreview PreviewAbility(this AbilityExecContext ctx)
    {
        var unmetRequirements = ctx.Definition.Components
            .OfType<IAbilityRequirementComponent>()
            .Where(arc => !arc.IsFulfilled(ctx)).ToArray();

        return new AbilityPreview
        {
            CanUse = unmetRequirements.Length == 0,
            Commands = ctx.Definition.Components
                .OfType<IAbilityCommandComponent>()
                .SelectMany(ac => ac.GetContribution(ctx))
                .ToArray(),
            Definition = ctx.Definition,
            UnfulfilledRequirements = unmetRequirements,
        };
    }
}