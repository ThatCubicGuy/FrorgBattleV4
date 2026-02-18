using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.BattleSystem.Actions;
using FrogBattleV4.Core.BattleSystem.Selections;

namespace FrogBattleV4.Core.BattleSystem;

public class BattleManager
{
    private readonly ISelectionProvider _playerInputInterface;
    private readonly ActionBarItem[] _actionBar;
    private readonly Random _rng = new();
    public List<Team> AllTeams { get; init; }
    [NotNull] public IOrderedEnumerable<ActionBarItem> ActionBar => _actionBar.Order();

    public BattleManager(ISelectionProvider provider, params Team[] teams)
    {
        AllTeams = teams.ToList();
        _playerInputInterface = provider;
        _actionBar = AllTeams
            .SelectMany(team => team.Members)
            .SelectMany(tt => tt.Turns.Select(action => new ActionBarItem(action)))
            .ToArray();
    }

    // public event EventHandler<BattleMember> OnMemberAdded;
    // public event EventHandler<BattleMember> OnMemberRemoved;
    public event EventHandler<BattleContext> TurnStart;
    public event EventHandler<BattleContext> TurnPlay;
    public event EventHandler<BattleContext> TurnEnd;
    
    public async Task RunAsync()
    {
        var turnNumber = 0U;
        while (true)
        {
            TryGetNextTurn(out var next);
            var member = next.TurnAction.Entity;
            var allyTeam = next.TurnAction.Entity.GetAlliedTeam(AllTeams);
            var ctx = new BattleContext
            {
                Manager = this,
                ActiveMember = member,
                Allies = allyTeam?.Members,
                Enemies = AllTeams.SelectMany(team => team.Members).Except(allyTeam?.Members ?? []),
                ActionOrder = ActionBar,
                TurnNumber = ++turnNumber,
                Rng = _rng
            };
            TurnStart?.Invoke(this, ctx);
            await next.TurnAction.PlayTurn(_playerInputInterface, ctx);
            TurnEnd?.Invoke(this, ctx);
        }
        // TODO: Win logic lmao
    }
    
    public bool TryGetNextTurn(out ActionBarItem action)
    {
        if (_actionBar.Length == 0)
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
        ActionBar.First(action => action.TurnAction.Entity == target).AdvancePercentage(percentage);
    }
}