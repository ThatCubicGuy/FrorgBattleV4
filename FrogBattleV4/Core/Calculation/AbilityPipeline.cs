using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.AbilitySystem.Components;

namespace FrogBattleV4.Core.Calculation;

internal static class AbilityPipeline
{
    [Pure]
    private static IEnumerable<IBattleCommand> GetCommands(this AbilityExecContext ctx)
    {
        return ctx.Definition.Components.OfType<IAbilityCommandComponent>().SelectMany(ac => ac.GetContribution(ctx));
    }

    [Pure]
    public static AbilityPreview PreviewAbility(this AbilityExecContext ctx)
    {
        var commands = ctx.GetCommands().ToArray();
        var unmetRequirements = ctx.Definition.Components
            .OfType<IAbilityRequirementComponent>()
            .Where(arc => !arc.IsFulfilled(ctx)).ToArray();

        return new AbilityPreview
        {
            CanUse = unmetRequirements.Length == 0,
            Commands = commands,
            UnfulfilledRequirements = unmetRequirements,
        };
    }

    public static bool ExecuteAbility(this AbilityExecContext ctx)
    {
        var preview = ctx.PreviewAbility();
        if (!preview.CanUse) return false;

        foreach (var command in preview.Commands)
        {
            command.ExecuteCommand(new ModifierContext
            {
                Actor = ctx.User,
                Other = ctx.MainTarget,
                Ability = ctx.Definition,
                Rng = ctx.Rng,
            });
        }

        return true;
    }
}