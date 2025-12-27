using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.BattleSystem.Decisions;

namespace FrogBattleV4.Core.CharacterSystem;

public class CharacterTurn(Character owner) : IAction
{
    private AbilityExecContext _abilityExecContext;
    public Character Owner { get; } = owner;
    public TurnContext TurnStatus { get; private set; } = new()
    {
        Moment = TurnMoment.None
    };

    public double BaseActionValue => 10000 / Owner.GetStat(nameof(Stat.Spd));

    public IDecisionRequester[] Decisions => [AbilityRequester, TargetRequester];
    public AbilityDecisionRequester AbilityRequester { get; } = new AbilityDecisionRequester(owner);
    public TargetDecisionRequester TargetRequester { get; } = new TargetDecisionRequester(owner);
    IBattleMember IAction.Entity => Owner;

    [Pure] public bool CanTakeAction(BattleContext ctx) => Owner.CanTakeAction(ctx);

    public void StartAction(BattleContext ctx)
    {
        TurnStatus = new TurnContext
        {
            TurnNumber = ctx.TurnNumber,
            Moment = TurnMoment.Start
        };
        _abilityExecContext = new AbilityExecContext
        {
            Rng = ctx.Rng,
            User = Owner,
            ValidTargets = ctx.Enemies.SelectMany(x => x.Parts).ToList().AsReadOnly()
        };
        Owner.InitTurn(ctx);
        TurnStatus = new TurnContext
        {
            TurnNumber = ctx.TurnNumber,
            Moment = TurnMoment.Selection
        };
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