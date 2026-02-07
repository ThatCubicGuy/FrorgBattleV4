#nullable enable
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.AbilitySystem.Components;
using FrogBattleV4.Core.BattleSystem;
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

    public double BaseActionValue => 10000 / Owner.GetStat(nameof(Stat.Spd));

    IBattleMember IAction.Entity => Owner;

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
        var targets = definition.TargetingType switch
        {
            TargetingType.None => null,
            TargetingType.Allies => ctx.Allies?.SelectMany(bm => bm.Parts),
            TargetingType.Enemies => ctx.Enemies?.SelectMany(bm => bm.Parts),
            // Oh, god...
            TargetingType.Both => (ctx.Allies ?? []).Concat(ctx.Enemies ?? [])
                .SelectMany(bm => bm.Parts),
            TargetingType.Self => [Owner.Body],
            TargetingType.Arena => throw new NotImplementedException("Arena targeting not implemented."),
            _ => throw new InvalidEnumArgumentException("TargetingType", (int)definition.TargetingType,
                typeof(TargetingType))
        };
        var targetResult = await provider.GetSelectionAsync(
            new TargetSelectionRequest(ctx.ActiveMember, targets));
        var mainTarget = targetResult.Choices.Single();
        var execCtx = new AbilityExecContext
        {
            User = Owner,
            Definition = definition,
            MainTarget = mainTarget,
            ValidTargets = targets,
            Rng = ctx.Rng
        };
        execCtx.ExecuteAbility();
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