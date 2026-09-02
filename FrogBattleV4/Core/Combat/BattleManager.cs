using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FrogBattleV4.Core.Combat.Actions;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Combat;

// TODO: Full revamp
public class BattleManager
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
        // todo
    }
}