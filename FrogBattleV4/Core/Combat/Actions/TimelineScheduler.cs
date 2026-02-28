using System;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Combat.Actions;

public class TimelineScheduler
{
    private readonly PriorityQueue<ScheduledAction, PriorityKey> _queue = new();
    private long _globalSequence;
    private double _now;

    public void Advance(ScheduledAction action, double flatValue)
    {
        _queue.Remove(action, out _, out var currentPriority);
        _queue.Enqueue(action, currentPriority with { Time = Math.Max(_now, currentPriority.Time - flatValue) });
    }

    public void Schedule(ScheduledAction action)
    {
        _queue.Enqueue(action, new PriorityKey(false, _now + action.BaseActionValue, _globalSequence++));
    }

    public void ScheduleRange(params ScheduledAction[] actions)
    {
        foreach (var action in actions)
        {
            _queue.Enqueue(action, new PriorityKey(false, _now + action.BaseActionValue, _globalSequence++));
        }
    }

    public void ScheduleInstant(ScheduledAction action)
    {
        _queue.Enqueue(action, new PriorityKey(true, _now, _globalSequence++));
    }

    public ScheduledAction MoveNext()
    {
        if (!_queue.TryDequeue(out var action, out var priority))
            throw new InvalidOperationException("There are no actions available");
        _now = priority.Time;
        return action;
    }

    public IEnumerable<ScheduledAction> GetOrderedActions()
    {
        return _queue.UnorderedItems
            .OrderBy(tuple => tuple.Priority)
            .Select(tuple => tuple.Element);
    }

    private readonly record struct PriorityKey(bool IsInstant, double Time, long Sequence) : IComparable<PriorityKey>
    {
        public int CompareTo(PriorityKey other)
        {
            var isInstantComparison = IsInstant.CompareTo(other.IsInstant);
            if (isInstantComparison != 0) return isInstantComparison;
            var timeComparison = Time.CompareTo(other.Time);
            if (timeComparison != 0) return timeComparison;
            var sequenceComparison = Sequence.CompareTo(other.Sequence);
            return sequenceComparison;
        }
    }
}