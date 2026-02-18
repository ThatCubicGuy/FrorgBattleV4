#nullable enable
using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.AbilitySystem.Components;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.BattleSystem.Actions;
using FrogBattleV4.Core.BattleSystem.Selections;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Calculation.Pools;

namespace FrogBattleV4.Core.CharacterSystem;

public class CharacterTurn(IBattleMember owner) : IAction
{
    public IBattleMember Entity { get; } = owner;

    public TurnContext TurnStatus { get; private set; } = new()
    {
        Moment = TurnMoment.None
    };

    public double BaseActionValue => 10000 / new ModifierContext(Entity).ComputeStat(StatId.Spd);
    
    [Pure]
    public bool CanTakeAction(BattleContext ctx)
    {
        return Entity.Pools.Any(pc => pc.HasAnyTags(PoolTag.Stuns));
        // return Owner == ctx.ActiveMember;
    }

    /// <summary>
    /// Runs effect calculations (such as DoT and general effect turn ticks)
    /// </summary>
    /// <param name="ctx">The context in which to start the turn.</param>
    public void StartTurn(BattleContext ctx)
    {
        Entity.Effects.TickStart();
        Entity.Pools.TickStart();
    }

    public void EndTurn()
    {
        Entity.Effects.TickEnd();
        Entity.Pools.TickEnd();
    }

    public async Task PlayTurn(ISelectionProvider provider, BattleContext ctx)
    {
        var defResult = await provider.GetSelectionAsync(
            new AbilitySelectionRequest(ctx.ActiveMember, ctx.ActiveMember.Abilities));
        var definition = defResult.Choices.Single();
        var targets = (definition.TargetingPool switch
        {
            TargetingPool.None => null,
            TargetingPool.Allies => ctx.Allies,
            TargetingPool.Enemies => ctx.Enemies,
            // Oh, god...
            TargetingPool.Both => (ctx.Allies ?? []).Concat(ctx.Enemies ?? []),
            TargetingPool.Self => [Entity],
            TargetingPool.Arena => throw new NotImplementedException("Arena targeting not implemented."),
            _ => throw new InvalidEnumArgumentException("TargetingType", (int)definition.TargetingPool,
                typeof(TargetingPool))
        })?.ToArray();
        var targetResult = await provider.GetSelectionAsync(
            new TargetSelectionRequest(ctx.ActiveMember, targets));
        var mainTarget = targetResult.Choices.Single();

        new AbilityExecContext
        {
            User = Entity,
            Definition = definition,
            MainTarget = mainTarget,
            ValidTargets = targets,
            Rng = ctx.Rng,
        }.ExecuteAbility();
    }
}