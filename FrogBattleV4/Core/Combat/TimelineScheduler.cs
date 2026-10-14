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
    public EntityUid Current { get; private set; }

    /// <summary>
    /// Advances every turn of an entity by a percentage of its base action value.
    /// </summary>
    /// <param name="entity">Entity UID.</param>
    /// <param name="advancePercent">Percentage to advance by.</param>
    public void AdvanceEntity(EntityUid entity, double advancePercent)
    {
        foreach (var turn in _queue.UnorderedItems.Select(tuple => tuple.Element).Where(turn => turn.Actor == entity))
        {
            Advance(turn, advancePercent);
        }
    }

    /// <summary>
    /// Advance a turn in the timeline by a percentage of its base turn value.
    /// </summary>
    /// <param name="turn">Turn to advance. Must be part of the timeline.</param>
    /// <param name="percentValue">Percentage to advance by.</param>
    /// <exception cref="ArgumentException">Turn is not part of the timeline.</exception>
    private void Advance(ScheduledTurn turn, double percentValue)
    {
        if (!_queue.Remove(turn, out turn!, out var currentPriority))
            throw new ArgumentException("Turn does not exist!", nameof(turn));
        _queue.Enqueue(turn, currentPriority with
        {
            ActionValue = Math.Max(_now, currentPriority.ActionValue - turn.BaseActionValue * percentValue)
        });
    }

    /// <summary>
    /// Schedules the given entity to act after <paramref name="baseActionValue"/>.
    /// </summary>
    /// <param name="entity">Entity UID.</param>
    /// <param name="baseActionValue">Action value after which the entity will act.</param>
    public void Schedule(EntityUid entity, double baseActionValue)
    {
        _queue.Enqueue(new ScheduledTurn(entity, baseActionValue),
            new PriorityKey(_now + baseActionValue, _globalSequence++));
    }

    /// <summary>
    /// Clears the turn queue.
    /// </summary>
    public void Clear()
    {
        _queue.Clear();
    }

    /// <summary>
    /// Dequeues a turn from the timeline and sets it as the current turn.
    /// </summary>
    /// <returns>True if there are still turns remaining, false if there is nothing left.</returns>
    public bool MoveNext()
    {
        if (!_queue.TryDequeue(out var turn, out var priority))
            return false;
        Current = turn.Actor;
        _now = priority.ActionValue;
        return true;
    }

    public IOrderedEnumerable<TimelineItem> GetOrderedActions()
    {
        return _queue.UnorderedItems
            .Select(tuple => new TimelineItem(tuple.Element.Actor, tuple.Priority.ActionValue - _now))
            .OrderBy(item => item.CurrentActionValue);
    }

    public record TimelineItem(EntityUid Actor, double CurrentActionValue);

    private readonly record struct ScheduledTurn(EntityUid Actor, double BaseActionValue);
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