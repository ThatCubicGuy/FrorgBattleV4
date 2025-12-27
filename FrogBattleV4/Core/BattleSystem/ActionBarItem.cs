#nullable enable
using System;

namespace FrogBattleV4.Core.BattleSystem;

public class ActionBarItem(IAction action) : IComparable<ActionBarItem?>
{
    private double _actionValue = action.BaseActionValue;

    public IAction TurnEvent { get; } = action;
    public double ActionValue
    {
        get => _actionValue;
        private set
        {
            _actionValue = value;
            if (_actionValue < 0) _actionValue = 0;
        }
    }

    public void Advance(double value)
    {
        ActionValue -= value;
    }

    public void AdvancePercentage(double value)
    {
        ActionValue -= TurnEvent.BaseActionValue * value;
    }
    
    public int CompareTo(ActionBarItem? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return -1;
        return ActionValue.CompareTo(other.ActionValue);
    }
}