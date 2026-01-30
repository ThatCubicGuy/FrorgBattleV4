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
using FrogBattleV4.Core.BattleSystem.Decisions;

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

    [Pure] public bool CanTakeAction(BattleContext ctx) => Owner.CanTakeAction(ctx);

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
    
    public async Task PlayTurn(IDecisionProvider provider, BattleContext ctx)
    {
        var defResult = await provider.GetSelectionAsync(
            new AbilityDecisionRequest(ctx.ActiveMember, (ctx.ActiveMember as ICharacter)!.Abilities));
        var def = defResult.Choices.Single();
        var targets = def.TargetingType switch
        {
            TargetingType.None => null,
            TargetingType.Allies => ctx.Allies?.SelectMany(x => x.Parts).ToList().AsReadOnly(),
            TargetingType.Enemies => ctx.Enemies?.SelectMany(x => x.Parts).ToList().AsReadOnly(),
            // Oh god
            TargetingType.Both => ((IBattleMember[])[]).Concat(ctx.Allies ?? []).Concat(ctx.Enemies ?? []).SelectMany(x => x.Parts).ToList().AsReadOnly(),
            TargetingType.Self => Array.AsReadOnly([Owner.Body]),
            TargetingType.Arena => throw new NotImplementedException("Arena targeting not implemented."),
            _ => throw new InvalidEnumArgumentException("TargetingType", (int)def.TargetingType,
                typeof(TargetingType))
        };
        var targetResult = await provider.GetSelectionAsync(
            new TargetDecisionRequest(ctx.ActiveMember, targets));
        var mainTarget = targetResult.Choices.Single();
        var execCtx = new AbilityExecContext
        {
            User = Owner,
            Definition = def,
            MainTarget = mainTarget,
            ValidTargets = targets,
            Rng = ctx.Rng
        };
        Owner.ExecuteAbility(execCtx);
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