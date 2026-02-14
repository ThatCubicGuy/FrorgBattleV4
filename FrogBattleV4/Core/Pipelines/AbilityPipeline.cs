using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.Pipelines;

internal static class AbilityPipeline
{
    [Pure]
    public static AbilityPreview PreviewAbility(this AbilityExecContext ctx)
    {
        var unmetRequirements = ctx.Definition.Requirements
            .Where(rc => !rc.IsFulfilled(ctx)).ToArray();

        var costs = ctx.Definition.Costs?
            .SelectMany(cc => cc.GetCostRequests(ctx))
            .Select(mr => mr.PreviewMutation(
                new MutationExecContext
                {
                    Holder = ctx.User,
                    Other = ctx.MainTarget
                })).ToArray();

        var damages = ctx.Definition.Attacks
            .SelectMany(ac => ac.GetDamageRequests(ctx))
            .Select(dr => dr.PreviewDamage(
                new DamageExecContext
                {
                    Source = ctx.User,
                    Other = dr.Target as BattleMember,
                    Rng = ctx.Rng
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
        foreach (var request in ctx.Definition.Costs.SelectMany(cc => cc.GetCostRequests(ctx)))
        {
            request.ExecuteMutation(new MutationExecContext
            {
                Holder = ctx.User,
                Other = ctx.MainTarget
            });
        }

        foreach (var dr in ctx.Definition.Attacks.SelectMany(ac => ac.GetDamageRequests(ctx)))
        {
            dr.ExecuteDamage(new DamageExecContext
            {
                Source = ctx.User,
                Other = dr.Target as BattleMember,
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