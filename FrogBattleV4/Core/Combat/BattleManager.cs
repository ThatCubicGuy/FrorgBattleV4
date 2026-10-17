using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    private readonly List<Fighter> _battleMembers = [];

    public event EventHandler? BattleStarted;
    public event EventHandler? BattleEnded;
    public event EventHandler<EffectAddedArgs>? EffectAdded;
    public event EventHandler<EffectAddedArgs>? EffectResisted;

    public BattleManager()
    {
        StartTime = DateTime.UtcNow.Ticks;
        _random = new Random(StartTime.GetHashCode());
        throw new NotImplementedException();
    }

    public long StartTime { get; }
    public TurnScheduler Scheduler { get; } = new();
    public ImmutableDictionary<EntityUid, FighterData> MemberData { get; private set; }
    public ImmutableDictionary<TeamUid, Team> Teams { get; private set; }

    public async Task RunAsync()
    {
        throw new NotImplementedException();
    }

    public BattleEnvironment GetEnvironment()
    {
        return new BattleEnvironment(MemberData, Teams, _random);
    }

    public void Visit(DamageCommand command)
    {
        var data = command.Data;
        var damage = command.NormalAmount;
        if (command.CriticalChance > _random.NextDouble())
        {
            damage = command.CriticalAmount;
        }

        MemberData[command.Relation.Target].TakeDamage(damage, data);
    }

    public void Visit(MutateCommand command)
    {
        var data = command.Data;
        MemberData[command.Relation.Target].Pools[data.TargetPool].Value += command.TotalAmount;
    }

    public void Visit(ApplyEffectCommand command)
    {
        var data = command.Data;
        var args = new EffectAddedArgs
        {
            Definition = data.Effect,
            Stacks = data.AddedStacks,
            Turns = data.InitialTurns
        };
        if (!(command.ApplicationChance > _random.NextDouble()))
        {
            EffectResisted?.Invoke(this, args);
            return;
        }

        MemberData[command.Relation.Target].AddStatusEffect(new StatusEffectInstance
        {
            Definition = data.Effect,
            Stacks = data.AddedStacks,
            Turns = data.InitialTurns
        });
        EffectAdded?.Invoke(this, args);
    }

    public void Visit(RemoveEffectCommand command)
    {
        var data = command.Data;
        foreach (var effect in MemberData[command.Relation.Target].StatusEffects.Where(data.Predicate))
        {
            if (effect.Definition.Properties.HasFlag(EffectFlags.Unremovable)) continue;

            MemberData[command.Relation.Target].RemoveStatusEffect(effect.Definition.Id);
        }
    }

    public void Visit(AdvanceTurnCommand command)
    {
        var data = command.Data;
        Scheduler.AdvanceEntity(command.Relation.Target, data.AdvancePercent);
    }

    public void Visit(QueueTurnCommand command)
    {
        var data = command.Data;
        Scheduler.Schedule(command.Relation.Target, data.BaseActionValue);
    }
}

public class CommandLaunchingArgs : EventArgs
{
    public required BattleCommand Command { get; init; }
}

public class ActionLaunchedArgs : EventArgs
{
    public required BattleCommand Command { get; init; }
}