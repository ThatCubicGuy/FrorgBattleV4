using System;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Combat.Actions;

public class TimelineScheduler
{
    private readonly PriorityQueue<IScheduledAction, PriorityKey> _queue = new();
    private long _globalSequence;
    private double _now;

    /// <summary>
    /// Cached current action.
    /// </summary>
    public IScheduledAction Current { get; private set; }

    public void Advance(IScheduledAction action, double flatValue)
    {
        _queue.Remove(action, out action, out var currentPriority);
        _queue.Enqueue(action, currentPriority with { Time = Math.Max(_now, currentPriority.Time - flatValue) });
    }

    public void Schedule(IScheduledAction action)
    {
        _queue.Enqueue(action, new PriorityKey(false, _now + action.BaseActionValue, _globalSequence++));
    }

    public void ScheduleInstant(IScheduledAction action)
    {
        _queue.Enqueue(action, new PriorityKey(true, _now, _globalSequence++));
    }

    public void ScheduleRange(IEnumerable<IScheduledAction> actions)
    {
        foreach (var action in actions)
        {
            Schedule(action);
        }
    }

    /// <summary>
    /// Dequeues an action from the actionbar and sets it as the current action.
    /// </summary>
    /// <returns>True if there are still actions remaining, false if there is nothing left.</returns>
    public bool MoveNext()
    {
        if (!_queue.TryDequeue(out var action, out var priority))
            return false;
        Current = action;
        _now = priority.Time;
        return true;
    }

    public IEnumerable<TimelineItem> GetOrderedActions()
    {
        return _queue.UnorderedItems
            .OrderBy(tuple => tuple.Priority)
            .Select(tuple => new TimelineItem(tuple.Element, tuple.Priority.Time - _now));
    }

    public readonly record struct TimelineItem(IScheduledAction Action, double CurrentActionValue);

    private readonly record struct PriorityKey(bool IsInstant, double Time, long Sequence) : IComparable<PriorityKey>
    {
        public int CompareTo(PriorityKey other)
        {
            // If IsInstant is true, we want this instance to be considered earlier than the other.
            var isInstantComparison = -IsInstant.CompareTo(other.IsInstant);
            if (isInstantComparison != 0) return isInstantComparison;
            var timeComparison = Time.CompareTo(other.Time);
            if (timeComparison != 0) return timeComparison;
            var sequenceComparison = Sequence.CompareTo(other.Sequence);
            return sequenceComparison;
        }
    }
}