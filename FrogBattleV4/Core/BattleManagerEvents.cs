using System;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core;

// Note: There are some inconsistencies in event naming relating to who is the subject
public sealed partial class BattleManager
{
    public event EventHandler? BattleStarted;
    public event EventHandler? BattleEnded;
    public event EventHandler<TurnEventArgs>? TurnStarted; 
    // the target took damage from the sender
    public event EventHandler<DamageTakenEventArgs>? DamageTaken;
    // the target received healing from the sender
    public event EventHandler<HealingReceivedEventArgs>? HealingReceived;
    // the sender mutated the pool of the target
    public event EventHandler<PoolMutatedEventArgs>? PoolMutated;
    // the sender applied an effect to the target
    public event EventHandler<EffectAppliedEventArgs>? EffectApplied;
    // the sender removed an effect from the target
    public event EventHandler<EffectRemovedEventArgs>? EffectRemoved; 
    // the target resisted an effect from the sender
    public event EventHandler<EffectResistedEventArgs>? EffectResisted;
    // the sender advanced the turn of the target
    public event EventHandler<TurnAdvancedEventArgs>? TurnAdvanced;
    // the sender queued a turn for the target
    public event EventHandler<ActionEnqueuedEventArgs>? ActionEnqueued;

    private void OnBattleStarted()
    {
        BattleStarted?.Invoke(this, EventArgs.Empty);
    }

    private void OnBattleEnded()
    {
        BattleEnded?.Invoke(this, EventArgs.Empty);
    }

    private void OnTurnStarted(TurnEventArgs e)
    {
        TurnStarted?.Invoke(this, e);
    }

    private void OnDamageTaken(DamageTakenEventArgs e)
    {
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

    private void OnActionEnqueued(ActionEnqueuedEventArgs e)
    {
        ActionEnqueued?.Invoke(this, e);
    }
}

public class TurnEventArgs : EventArgs
{
    public required EntityUid Actor { get; init; }
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
    public required StatusEffectId EffectId { get; init; }
    public required StatusEffectType EffectType { get; init; }
    public required int Turns { get; init; }
    public required int Stacks { get; init; }
}

public class EffectRemovedEventArgs : BattleCommandEventArgs
{
    public required StatusEffectId EffectId { get; init; }
    public required int TurnsLeft { get; init; }
    public required int Stacks { get; init; }
}

public class EffectResistedEventArgs : BattleCommandEventArgs
{
    public required StatusEffectId EffectId { get; init; }
}

public class TurnAdvancedEventArgs : BattleCommandEventArgs
{
    public required double AdvanceAmount { get; init; }
}

public class ActionEnqueuedEventArgs : BattleCommandEventArgs;