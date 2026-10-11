using System;
using System.Threading.Tasks;
using FrogBattleV4.Core.Combat.Actions;

namespace FrogBattleV4.Core.Combat;

// TODO: Full revamp
public class BattleManager : IActionVisitor
{
    private Random Random { get; }
    private long StartTime { get; }

    public BattleManager()
    {
        StartTime = DateTime.UtcNow.Ticks;
        Random = new Random(StartTime.GetHashCode());
        throw new NotImplementedException();
    }

    public TimelineScheduler Scheduler { get; } = new();

    public async Task RunAsync()
    {
        throw new NotImplementedException();
    }

    public void Visit(Damage action)
    {
        throw new NotImplementedException();
    }

    public void Visit(Mutate action)
    {
        throw new NotImplementedException();
    }

    public void Visit(ApplyEffect action)
    {
        throw new NotImplementedException();
    }

    public void Visit(RemoveEffect action)
    {
        throw new NotImplementedException();
    }

    public void Visit(AdvanceTurn action)
    {
        throw new NotImplementedException();
    }

    public void Visit(QueueTurn action)
    {
        throw new NotImplementedException();
    }
}