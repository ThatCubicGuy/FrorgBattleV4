using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem.Decisions;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem;

public class BattleManager
{
    private readonly IDecisionProvider _playerInputInterface;
    private readonly ActionBarItem[] _actionBar;
    private readonly Random _rng = new();
    public Team Team1 { get; }
    public Team Team2 { get; }
    [NotNull] public IOrderedEnumerable<ActionBarItem> ActionBar => _actionBar.Order();

    public BattleManager(Team team1, Team team2, IDecisionProvider provider)
    {
        Team1 = team1;
        Team2 = team2;
        _playerInputInterface = provider;
        _actionBar = team1.Members.Concat(team2.Members).SelectMany(x => x.Turns?.Select(y => new ActionBarItem(y))).ToArray();
    }
    
    public async Task RunAsync()
    {
        while (TryGetNextTurn(out var next))
        {
            var ctx = new BattleContext
            {
                Allies = Team1.Members.AsReadOnly(),
                Enemies = Team2.Members.AsReadOnly(),
                Rng = _rng
            };
            next.TurnEvent.StartAction(ctx);
            if (next.TurnEvent.CanTakeAction(ctx))
            {
                if (next.TurnEvent.Decisions is { } decisions && next.TurnEvent is CharacterTurn turn)
                {
                    var ability = await _playerInputInterface.GetSelectionAsync(turn.AbilityRequester);
                    var target = await _playerInputInterface.GetSelectionAsync(turn.TargetRequester);
                    turn.Owner.ExecuteAbility(new AbilityExecContext
                    {
                        Definition = ability,
                        MainTarget = target,
                        Rng = _rng,
                        User = turn.Owner,
                        ValidTargets = ctx.Enemies.SelectMany(x => x.Parts).ToList().AsReadOnly()
                    });
                }
            }
            next.TurnEvent.EndAction(ctx);
        }
    }
    
    protected void AlignActionBar()
    {
        var min = _actionBar.MinBy(x => x.ActionValue).ActionValue;
        foreach (var item in _actionBar)
        {
            item.Advance(min);
        }
    }
    
    public bool TryGetNextTurn(out ActionBarItem action)
    {
        if (!ActionBar.Any())
        {
            action = null;
            return false;
        }
        var next = ActionBar.First();
        foreach (var item in _actionBar)
        {
            item.Advance(next.ActionValue);
        }
        action = next;
        return true;
    }

    public void AdvanceTarget(IBattleMember target, double percentage)
    {
        
    }
}