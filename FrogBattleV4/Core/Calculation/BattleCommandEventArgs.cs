using System;

namespace FrogBattleV4.Core.Calculation;

public abstract class BattleCommandEventArgs : EventArgs, IBattleCommandEventArgs
{
    public required EntityUid Sender { get; init; }
    public required EntityUid Target { get; init; }
}

// used for specifying type constraint on other IEventArgs interfaces
#pragma warning disable CA1711 // EventArgs suffix here is unambiguous
public interface IBattleCommandEventArgs
#pragma warning restore CA1711
{
    EntityUid Sender { get; }
    EntityUid Target { get; }
}