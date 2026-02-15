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
using FrogBattleV4.Core.Pipelines;

namespace FrogBattleV4.Core.CharacterSystem;

public class CharacterTurn(Character owner) : IAction
{
    public Character Owner { get; } = owner;

    public TurnContext TurnStatus { get; private set; } = new()
    {
        Moment = TurnMoment.None
    };

    public double BaseActionValue => 10000 / Owner.GetStat(StatId.Spd);

    BattleMember IAction.Entity => Owner;

    [Pure]
    public bool CanTakeAction(BattleContext ctx) => Owner.CanTakeAction(ctx);

    public void StartAction(BattleContext ctx)
    {
        TurnStatus = new TurnContext
        {
            TurnNumber = ctx.TurnNumber,
            Moment = TurnMoment.Start
        };
        Owner.InitTurn(ctx);
        TurnStatus = new TurnContext
        {
            TurnNumber = ctx.TurnNumber,
            Moment = TurnMoment.Selection
        };
    }

    public async Task PlayTurn(ISelectionProvider provider, BattleContext ctx)
    {
        var defResult = await provider.GetSelectionAsync(
            new AbilitySelectionRequest(ctx.ActiveMember, (ctx.ActiveMember as Character)!.Abilities));
        var definition = defResult.Choices.Single();
        var targets = (definition.TargetingPool switch
        {
            TargetingPool.None => null,
            TargetingPool.Allies => ctx.Allies,
            TargetingPool.Enemies => ctx.Enemies,
            // Oh, god...
            TargetingPool.Both => (ctx.Allies ?? []).Concat(ctx.Enemies ?? []),
            TargetingPool.Self => [Owner],
            TargetingPool.Arena => throw new NotImplementedException("Arena targeting not implemented."),
            _ => throw new InvalidEnumArgumentException("TargetingType", (int)definition.TargetingPool,
                typeof(TargetingPool))
        })?.ToArray();
        var targetResult = await provider.GetSelectionAsync(
            new TargetSelectionRequest(ctx.ActiveMember, targets));
        var mainTarget = targetResult.Choices.Single();

        new AbilityExecContext
        {
            User = Owner,
            Definition = definition,
            MainTarget = mainTarget,
            ValidTargets = targets,
            Rng = ctx.Rng,
        }.ExecuteAbility();
    }

    public void EndAction(BattleContext ctx)
    {
        TurnStatus = TurnStatus with
        {
            Moment = TurnMoment.End
        };
        Owner.FinalizeTurn();
        TurnStatus = TurnStatus with
        {
            Moment = TurnMoment.None
        };
    }
}