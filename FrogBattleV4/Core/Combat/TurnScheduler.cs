using System;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Combat;

public class TurnScheduler
{
    private readonly PriorityQueue<ScheduledTurn, PriorityKey> _turnTimeline = new();
    private readonly Queue<InstantAction> _actionTimeline = new();
    private long _globalSequence;
    private double _now;
    // Initialize with null! to retain nullability annotations when setting _current
    private IAction _current = null!;

    /// <summary>
    /// The last dequeued turn of the scheduler.
    /// </summary>
    public IAction Current => _current ??
                              throw new InvalidOperationException(
                                  "Cannot get current action before moving at least one turn.");

    /// <summary>
    /// Dequeues a turn from the timeline and sets it as the current turn.
    /// </summary>
    /// <returns>True if there are still turns remaining, false if there is nothing left.</returns>
    public bool MoveNext()
    {
        if (_actionTimeline.TryDequeue(out var action))
        {
            _current = action;
            return true;
        }

        if (!_turnTimeline.TryDequeue(out var turn, out var priority))
            return false;

        _current = turn;
        _now = priority.ActionValue;
        return true;
    }

    /// <summary>
    /// Advances every turn of an entity by a percentage of its base action value.
    /// </summary>
    /// <param name="entity">Entity UID.</param>
    /// <param name="advancePercent">Percentage to advance by.</param>
    public void AdvanceEntity(EntityUid entity, double advancePercent)
    {
        var turn = _turnTimeline.UnorderedItems
            .Select(tuple => tuple.Element)
            .SingleOrDefault(turn => turn.Actor == entity);

        if (turn is not null) Advance(turn, advancePercent);
    }

    /// <summary>
    /// Schedules the given turn.
    /// </summary>
    /// <param name="turn">Action to schedule.</param>
    public void Schedule(ScheduledTurn turn)
    {
        _turnTimeline.Enqueue(turn, new PriorityKey(_now + turn.BaseActionValue, _globalSequence++));
    }

    /// <summary>
    /// Enqueues an instant action into the action queue.
    /// </summary>
    /// <param name="action">Action to enqueue.</param>
    public void EnqueueInstant(InstantAction action)
    {
        _actionTimeline.Enqueue(action);
    }

    public IOrderedEnumerable<TimelineItem> GetOrderedActions()
    {
        return _turnTimeline.UnorderedItems
            .Select(tuple => new TimelineItem(tuple.Element.Actor, tuple.Priority.ActionValue - _now))
            .OrderBy(item => item.CurrentActionValue);
    }

    /// <summary>
    /// Advance a turn in the timeline by a percentage of its base turn value.
    /// </summary>
    /// <param name="turn">Turn to advance. Must be part of the timeline.</param>
    /// <param name="percentValue">Percentage to advance by.</param>
    /// <exception cref="ArgumentException">Turn is not part of the timeline.</exception>
    private void Advance(ScheduledTurn turn, double percentValue)
    {
        if (!_turnTimeline.Remove(turn, out turn!, out var priority))
            throw new ArgumentException("Turn does not exist!", nameof(turn));
        _turnTimeline.Enqueue(turn, priority with
        {
            ActionValue = Math.Max(_now, priority.ActionValue - turn.BaseActionValue * percentValue),
        });
    }

    public readonly record struct TimelineItem(EntityUid Actor, double CurrentActionValue);

    private readonly record struct PriorityKey(double ActionValue, long Sequence) : IComparable<PriorityKey>
    {
        public int CompareTo(PriorityKey other)
        {
            var timeComparison = ActionValue.CompareTo(other.ActionValue);
            if (timeComparison != 0) return timeComparison;
            var sequenceComparison = Sequence.CompareTo(other.Sequence);
            return sequenceComparison;
        }
    }
}

public enum TurnMoment
{
    Outside,
    Start,
    End,
}