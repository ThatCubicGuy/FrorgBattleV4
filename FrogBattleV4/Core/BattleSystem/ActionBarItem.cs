#nullable enable
using System;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem;

public class ActionBarItem(ITurn turn) : IComparable<ActionBarItem?>
{
    public ITurn TurnEvent { get; init; } = turn;
    public double ActionValue { get; protected set; } = turn.BaseActionValue;
    public TurnState TurnState { get; set; }

    public void Advance(double value)
    {
        ActionValue -= value;
        if (ActionValue < 0) ActionValue = 0;
    }
    
    public int CompareTo(ActionBarItem? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return -1;
        return ActionValue.CompareTo(other.ActionValue);
    }
}

public enum TurnState
{
    Inactive,
    Starting,
    Active,
    Ending
}