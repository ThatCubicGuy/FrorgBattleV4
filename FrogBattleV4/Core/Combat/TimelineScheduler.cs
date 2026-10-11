using System;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Combat;

public class TimelineScheduler
{
    private readonly PriorityQueue<ScheduledTurn, PriorityKey> _queue = new();
    private long _globalSequence;
    private double _now;

    /// <summary>
    /// The last dequeued action of the scheduler.
    /// </summary>
    public ScheduledTurn Current { get; private set; }

    /// <summary>
    /// Advance a turn in the timeline by a flat amount.
    /// </summary>
    /// <param name="turn">Turn to advance. Must be part of the timeline.</param>
    /// <param name="flatValue">Value to advance by.</param>
    /// <exception cref="ArgumentException">Turn is not part of the timeline.</exception>
    public void Advance(ScheduledTurn turn, double flatValue)
    {
        if (!_queue.Remove(turn, out turn!, out var currentPriority))
            throw new ArgumentException("Turn does not exist!", nameof(turn));
        _queue.Enqueue(turn, currentPriority with { Time = Math.Max(_now, currentPriority.Time - flatValue) });
    }

    /// <summary>
    /// Advance a turn in the timeline by a percentage of its base turn value.
    /// </summary>
    /// <param name="turn">Turn to advance. Must be part of the timeline.</param>
    /// <param name="percentValue">Percentage to advance by.</param>
    /// <exception cref="ArgumentException">Turn is not part of the timeline.</exception>
    public void AdvancePercent(ScheduledTurn turn, double percentValue)
    {
        Advance(turn, turn.BaseActionValue * percentValue);
    }

    public void Schedule(ScheduledTurn action)
    {
        _queue.Enqueue(action, new PriorityKey(_now + action.BaseActionValue, _globalSequence++));
    }

    public void ScheduleInstant(ScheduledTurn action)
    {
        _queue.Enqueue(action, new PriorityKey(_now, _globalSequence++));
    }

    public void ScheduleRange(IEnumerable<ScheduledTurn> actions)
    {
        foreach (var action in actions)
        {
            Schedule(action);
        }
    }

    /// <summary>
    /// Dequeues a turn from the timeline and sets it as the current turn.
    /// </summary>
    /// <returns>True if there are still turns remaining, false if there is nothing left.</returns>
    public bool MoveNext()
    {
        if (!_queue.TryDequeue(out var turn, out var priority))
            return false;
        Current = turn;
        _now = priority.Time;
        return true;
    }

    public IOrderedEnumerable<TimelineItem> GetOrderedActions()
    {
        return _queue.UnorderedItems
            .Select(tuple => new TimelineItem(tuple.Element, tuple.Priority.Time - _now))
            .OrderBy(item => item.CurrentActionValue);
    }

    public readonly record struct TimelineItem(ScheduledTurn Turn, double CurrentActionValue);

    private readonly record struct PriorityKey(double Time, long Sequence) : IComparable<PriorityKey>
    {
        public int CompareTo(PriorityKey other)
        {
            var timeComparison = Time.CompareTo(other.Time);
            if (timeComparison != 0) return timeComparison;
            var sequenceComparison = Sequence.CompareTo(other.Sequence);
            return sequenceComparison;
        }
    }
}