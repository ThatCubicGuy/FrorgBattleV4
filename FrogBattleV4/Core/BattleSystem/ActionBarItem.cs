#nullable enable
using System;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem;

public class ActionBarItem(ITakesTurn actor) : IComparable<ActionBarItem?>
{
    public required ITakesTurn Entity { get; init; } = actor;
    public double ActionValue { get; protected set; }

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