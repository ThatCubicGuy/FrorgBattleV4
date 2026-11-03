using System;

namespace FrogBattleV4.Core.Calculation;

public abstract class BattleCommandEventArgs : EventArgs, IBattleCommandEventArgs
{
    public required EntityUid Sender { get; init; }
    public required EntityUid Target { get; init; }
}

// used for specifying type constraint on other IEventArgs interfaces
public interface IBattleCommandEventArgs
{
    EntityUid Sender { get; }
    EntityUid Target { get; }
}