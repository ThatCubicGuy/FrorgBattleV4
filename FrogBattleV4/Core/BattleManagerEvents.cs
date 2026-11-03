using System;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core;

public sealed partial class BattleManager
{
    public event EventHandler? BattleStarted;
    public event EventHandler? BattleEnded;
    public event EventHandler<DamageTakenEventArgs>? DamageTaken;
    public event EventHandler<HealingReceivedEventArgs>? HealingReceived;
    public event EventHandler<PoolMutatedEventArgs>? PoolMutated;
    public event EventHandler<EffectAppliedEventArgs>? EffectApplied;
    public event EventHandler<EffectRemovedEventArgs>? EffectRemoved; 
    public event EventHandler<EffectResistedEventArgs>? EffectResisted;
    public event EventHandler<TurnAdvancedEventArgs>? TurnAdvanced;
    public event EventHandler<TurnQueuedEventArgs>? TurnQueued;

    private void OnBattleStarted()
    {
        BattleStarted?.Invoke(this, EventArgs.Empty);
    }

    private void OnBattleEnded()
    {
        BattleEnded?.Invoke(this, EventArgs.Empty);
    }

    private void OnDamageTaken(DamageTakenEventArgs e)
    {
        Logger.WriteLine($"{e.Amount} damage taken by {e.Target} from {e.Sender}!");
        DamageTaken?.Invoke(this, e);
    }

    private void OnHealingReceived(HealingReceivedEventArgs e)
    {
        HealingReceived?.Invoke(this, e);
    }

    private void OnPoolMutated(PoolMutatedEventArgs e)
    {
        PoolMutated?.Invoke(this, e);
    }

    private void OnEffectApplied(EffectAppliedEventArgs e)
    {
        EffectApplied?.Invoke(this, e);
    }

    private void OnEffectRemoved(EffectRemovedEventArgs e)
    {
        EffectRemoved?.Invoke(this, e);
    }

    private void OnEffectResisted(EffectResistedEventArgs e)
    {
        EffectResisted?.Invoke(this, e);
    }

    private void OnTurnAdvanced(TurnAdvancedEventArgs e)
    {
        TurnAdvanced?.Invoke(this, e);
    }

    private void OnTurnQueued(TurnQueuedEventArgs e)
    {
        TurnQueued?.Invoke(this, e);
    }
}

public class DamageTakenEventArgs : BattleCommandEventArgs, IDeltaEventArgs
{
    public required double Amount { get; init; }
    public required DamageData Data { get; init; }
    double IDeltaEventArgs.Delta => Amount;
}

public class HealingReceivedEventArgs : BattleCommandEventArgs, IDeltaEventArgs
{
    public required double Amount { get; init; }
    public required HealData Data { get; init; }
    double IDeltaEventArgs.Delta => Amount;
}

public class PoolMutatedEventArgs : BattleCommandEventArgs, IDeltaEventArgs
{
    public required PoolName Pool { get; init; }
    public required double OldValue { get; init; }
    public required double NewValue { get; init; }
    public double Delta => NewValue - OldValue;
}

public class EffectAppliedEventArgs : BattleCommandEventArgs
{
    public required StatusEffectId Id { get; init; }
    public required int Turns { get; init; }
    public required int Stacks { get; init; }
}

public class EffectRemovedEventArgs : BattleCommandEventArgs
{
    public required StatusEffectId Id { get; init; }
    public required int TurnsLeft { get; init; }
    public required int Stacks { get; init; }
}

public class EffectResistedEventArgs : BattleCommandEventArgs;

public class TurnAdvancedEventArgs : BattleCommandEventArgs
{
    public required double AdvanceAmount { get; init; }
}

public class TurnQueuedEventArgs : BattleCommandEventArgs;