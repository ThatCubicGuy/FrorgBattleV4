#nullable enable
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.AbilitySystem.Components;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Selections;

namespace FrogBattleV4.Core.Combat.Actions;

public interface IScheduledAction
{
    bool InstantRequeue { get; }
    IBattleMember Actor { get; }
    double BaseActionValue { get; }
    Task PlayTurn(ISelectionProvider provider, BattleContext ctx);
}

public static class ActionPipeline
{
    // TODO: Previews, errors, the whole nine yards
    // Also, this only handles regular turns.
    // Bonus turns don't exist yet.
    // There probably should be a separation there.
    // ALSO, the selection provider might need to be per action.
    // NPC Enemies shouldn't get the same provider as the player.
    public static async Task PlayTurn(this IScheduledAction action, ISelectionProvider provider, BattleContext ctx)
    {
        action.Actor.Effects.TickStart();
        action.Actor.Pools.TickStart();

        // Ability selection
        var defResult = await provider.GetSelectionAsync(
            new AbilitySelectionRequest(ctx.ActiveMember, ctx.ActiveMember.Abilities));

        var definition = defResult.Choices.Single();

        // Enumerate available targets
        var targets = (definition.TargetingPool switch
        {
            TargetingPool.None => null,
            TargetingPool.Allies => ctx.Allies,
            TargetingPool.Enemies => ctx.Enemies,
            // Oh, god...
            TargetingPool.Both => (ctx.Allies ?? []).Concat(ctx.Enemies ?? []),
            TargetingPool.Self => [action.Actor],
            TargetingPool.Arena => throw new NotImplementedException("Arena targeting not implemented."),
            _ => throw new InvalidEnumArgumentException("TargetingType", (int)definition.TargetingPool,
                typeof(TargetingPool))
        })?.ToArray();

        // Target selection
        var targetResult = await provider.GetSelectionAsync(
            new TargetSelectionRequest(ctx.ActiveMember, targets));
        var mainTarget = targetResult.Choices.First();

        // Execute ability
        new AbilityExecContext
        {
            User = action.Actor,
            Definition = definition,
            MainTarget = mainTarget,
            ValidTargets = targets,
            Rng = ctx.Rng,
        }.ExecuteAbility();

        action.Actor.Effects.TickEnd();
        action.Actor.Pools.TickEnd();
    }
}