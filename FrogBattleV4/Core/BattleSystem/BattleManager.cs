using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem.Selections;
using FrogBattleV4.Core.CharacterSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.Pipelines;

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
            .OfType<ITakesTurns>()
            .SelectMany(tt => tt.Turns.Select(action => new ActionBarItem(action)))
            .ToArray();
    }

    // public event EventHandler<BattleMember> OnMemberAdded;
    // public event EventHandler<BattleMember> OnMemberRemoved;
    public event EventHandler<BattleContext> OnTurnStart;
    public event EventHandler<BattleContext> OnTurnPlay;
    public event EventHandler<BattleContext> OnTurnEnd;
    
    public async Task RunAsync()
    {
        var turnNumber = 0U;
        while (TryGetNextTurn(out var next))
        {
            var member = next.TurnAction.Entity;
            var team = next.TurnAction.Entity.GetAlliedTeam(AllTeams);
            var ctx = new BattleContext
            {
                Manager = this,
                ActiveMember = member,
                Allies = team?.Members,
                Enemies = AllTeams.SelectMany(x => x.Members).Except(team?.Members ?? []).ToList(),
                ActionOrder = ActionBar,
                TurnNumber = ++turnNumber,
                Rng = _rng
            };
            OnTurnStart?.Invoke(this, ctx);
            next.TurnAction.StartAction(ctx);
            if (next.TurnAction.CanTakeAction(ctx))
            {
                OnTurnPlay?.Invoke(this, ctx);
                await next.TurnAction.PlayTurn(_playerInputInterface, ctx);
            }
            next.TurnAction.EndAction(ctx);
            OnTurnEnd?.Invoke(this, ctx);
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
        ActionBar.First(action => action.TurnAction.Entity == target).AdvancePercentage(percentage);
    }
}