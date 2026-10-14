using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.Combat.Actions;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Combat;

// TODO: Full revamp
public class BattleManager : IActionVisitor
{
    private readonly Random _random;
    private readonly Dictionary<EntityUid, Fighter> _battleMembers = [];

    public BattleManager()
    {
        StartTime = DateTime.UtcNow.Ticks;
        _random = new Random(StartTime.GetHashCode());
        throw new NotImplementedException();
    }

    public long StartTime { get; }
    public TimelineScheduler Scheduler { get; } = new();
    private Dictionary<EntityUid, FighterData> MemberData { get; } = [];
    private Dictionary<TeamUid, Team> Teams { get; } = [];

    public async Task RunAsync()
    {
        throw new NotImplementedException();
    }

    public BattleEnvironment GetEnvironment()
    {
        return new BattleEnvironment(MemberData, Teams, _random);
    }

    public void Visit(DamageAction action)
    {
        MemberData[action.Relation.Target].TakeDamage(action.TotalAmount, action.Data);
    }

    public void Visit(MutateAction action)
    {
        var data = action.Data;
        MemberData[action.Relation.Target].Pools[data.TargetPool].Value += action.TotalAmount;
    }

    public void Visit(ApplyEffectAction action)
    {
        var data = action.Data;
        MemberData[action.Relation.Target].AddStatusEffect(new StatusEffectInstance
        {
            Definition = data.Effect,
            Stacks = data.AddedStacks,
            Turns = data.InitialTurns
        });
    }

    public void Visit(RemoveEffectAction action)
    {
        // TODO (Nova)
        var data = action.Data;
        foreach (var effect in MemberData[action.Relation.Target].StatusEffects.Where(data.Predicate))
        {
            MemberData[action.Relation.Target].RemoveStatusEffect(effect.Definition.Id);
        }
    }

    public void Visit(AdvanceTurnAction action)
    {
        Scheduler.AdvanceEntity(action.Relation.Target, action.Data.AdvancePercent);
    }

    public void Visit(QueueTurnAction action)
    {
        Scheduler.Schedule(action.Relation.Target, action.Data.BaseActionValue);
    }
}