using System;
using System.Collections.Generic;
using System.Linq;

namespace FrogBattleV4.Core.Combat;

public class TimelineScheduler
{
    private readonly PriorityQueue<IScheduledTurn, PriorityKey> _queue = new();
    private long _globalSequence;
    private double _now;

    /// <summary>
    /// The last dequeued action of the scheduler.
    /// </summary>
    public IScheduledTurn Current { get; private set; }

    /// <summary>
    /// Advance an action in the timeline by a flat amount.
    /// </summary>
    /// <param name="action">Action to advance. Must be part of the timeline.</param>
    /// <param name="flatValue">Value to advance by.</param>
    /// <exception cref="ArgumentException">Action is not part of the timeline.</exception>
    public void Advance(IScheduledTurn action, double flatValue)
    {
        // Non-exception throwing guard against null actions
        if (action is null) return;
        if (!_queue.Remove(action, out action, out var currentPriority))
            throw new ArgumentException("Action does not exist!", nameof(action));
        _queue.Enqueue(action, currentPriority with { Time = Math.Max(_now, currentPriority.Time - flatValue) });
    }

    /// <summary>
    /// Advance an action in the timeline by a percentage of its base action value.
    /// </summary>
    /// <param name="action">Action to advance. Must be part of the timeline.</param>
    /// <param name="percentValue">Percentage to advance by.</param>
    /// <exception cref="ArgumentException">Action is not part of the timeline.</exception>
    public void AdvancePercent(IScheduledTurn action, double percentValue)
    {
        Advance(action, action.BaseActionValue * percentValue);
    }

    public void Schedule(IScheduledTurn action)
    {
        _queue.Enqueue(action, new PriorityKey(false, _now + action.BaseActionValue, _globalSequence++));
    }

    public void ScheduleInstant(IScheduledTurn action)
    {
        _queue.Enqueue(action, new PriorityKey(true, _now, _globalSequence++));
    }

    public void ScheduleRange(IEnumerable<IScheduledTurn> actions)
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

    public IOrderedEnumerable<TimelineItem> GetOrderedActions()
    {
        return _queue.UnorderedItems
            .Select(tuple => new TimelineItem(tuple.Element, tuple.Priority.Time - _now))
            .OrderBy(ti => ti.CurrentActionValue);
    }

    public readonly record struct TimelineItem(IScheduledTurn Action, double CurrentActionValue);

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