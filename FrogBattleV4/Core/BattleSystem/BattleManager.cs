using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FrogBattleV4.Core.BattleSystem;

public class BattleManager
{
    // private ActionBarItem[] _actionBar = new ActionBarItem[8];
    // public IOrderedEnumerable<ActionBarItem> ActionBar => _actionBar.Order();
    public IOrderedEnumerable<ActionBarItem> ActionBar { get; private set; }

    protected void AlignActionBar()
    {
        var min = ActionBar.First().ActionValue;
        foreach (var item in ActionBar)
        {
            item.Advance(min);
        }
    }
    
    public void NextTurn()
    {
        var min = ActionBar.First();
        foreach (var item in ActionBar.Skip(1))
        {
            item.Advance(min.ActionValue);
        }
        ActionBar = ActionBar = ActionBar.Skip(1).Append(min).Order();
    }
}