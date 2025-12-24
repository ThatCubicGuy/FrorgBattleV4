using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem;

public class BattleManager
{
    // private ActionBarItem[] _actionBar = new ActionBarItem[8];
    // public IOrderedEnumerable<ActionBarItem> ActionBar => _actionBar.Order();
    public Team Team1 { get; }
    public Team Team2 { get; }
    public IOrderedEnumerable<ActionBarItem> ActionBar { get; private set; }

    public BattleManager(Team team1, Team team2)
    {
        Team1 = team1;
        Team2 = team2;
        ActionBar = team1.Members.SelectMany(x => x.Turns.Select(y => new ActionBarItem(y))).Order();
    }
    
    protected void AlignActionBar()
    {
        var min = ActionBar.First().ActionValue;
        foreach (var item in ActionBar)
        {
            item.Advance(min);
        }
    }
    
    public bool TryGetNextTurn(out ITurn turn)
    {
        if (!ActionBar.Any())
        {
            turn = null;
            return false;
        }
        var min = ActionBar.First();
        foreach (var item in ActionBar.Skip(1))
        {
            item.Advance(min.ActionValue);
        }
        ActionBar = ActionBar.Skip(1).Append(min).Order();
        turn = min.TurnEvent;
        return true;
    }

    public void AdvanceTarget(ITargetable target, double percentage)
    {
        
    }
}