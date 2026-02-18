using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.Calculation;

internal static class AbilityPipeline
{
    [Pure]
    public static AbilityPreview PreviewAbility(this AbilityExecContext ctx)
    {
        var unmetRequirements = ctx.Definition.Requirements
            .Where(rc => !rc.IsFulfilled(ctx)).ToArray();

        var costs = ctx.Definition.Costs?
            .SelectMany(cc => cc.GetCostRequests(ctx))
            .Select(mi => mi.PreviewMutation(
                new ModifierContext(ctx.User, ctx.MainTarget))).ToArray();

        var damages = ctx.Definition.Attacks
            .SelectMany(ac => ac.GetDamageRequests(ctx))
            .Select(di => di.PreviewDamage(
                new DamageExecContext
                {
                    Source = ctx.User,
                    Definition = ctx.Definition,
                    Rng = ctx.Rng,
                })).ToArray();

        return new AbilityPreview
        {
            CanUse = ctx.CanExecute(),
            Mutations = costs,
            Damages = damages,
            UnfulfilledRequirements = unmetRequirements
        };
    }

    public static bool ExecuteAbility(this AbilityExecContext ctx)
    {
        if (!ctx.CanExecute()) return false;
        foreach (var mutationIntent in ctx.Definition.Costs.SelectMany(cc => cc.GetCostRequests(ctx)))
        {
            mutationIntent.ExecuteMutation(new ModifierContext
            {
                Actor = ctx.User,
                Other = ctx.MainTarget,
                Ability = ctx.Definition,
                Rng = ctx.Rng,
            });
        }

        foreach (var damageIntent in ctx.Definition.Attacks.SelectMany(ac => ac.GetDamageRequests(ctx)))
        {
            damageIntent.ExecuteDamage(new DamageExecContext
            {
                Source = ctx.User,
                Definition = ctx.Definition,
                Rng = ctx.Rng
            });
        }

        return true;
    }

    [Pure]
    private static bool CanExecute(this AbilityExecContext ctx)
    {
        return ctx.Definition.Requirements.All(item => item.IsFulfilled(ctx));
    }
}