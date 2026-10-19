using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Combat.Commands;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Modifiers.StatusEffects;
using FrogBattleV4.Core.Selections;

namespace FrogBattleV4.Core.Combat;

// TODO: Full revamp
public class BattleManager : IBattle
{
    private readonly SeededBattleRng _rng;
    private readonly CommandResolver _commandResolver;

    public event EventHandler? BattleStarted;
    public event EventHandler? BattleEnded;
    public event EventHandler<EffectAddedArgs>? EffectAdded;
    public event EventHandler<EffectAddedArgs>? EffectResisted;

    private BattleManager(ImmutableDictionary<EntityUid, Fighter> memberData, ImmutableDictionary<TeamUid, Team> teams)
    {
        StartTime = DateTime.UtcNow.Ticks;
        _rng = new SeededBattleRng(StartTime.GetHashCode());
        _commandResolver = new CommandResolver(this);
        MemberData = memberData;
        Teams = teams;
    }

    public long StartTime { get; private set; }
    public TurnScheduler Scheduler { get; } = new();

    public ImmutableDictionary<EntityUid, Fighter> MemberData { get; private set; }
    public ImmutableDictionary<TeamUid, Team> Teams { get; private set; }

    public static Builder GetBuilder()
    {
        return new Builder();
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        // TODO (Nova): incomplete
        BattleStarted?.Invoke(this, EventArgs.Empty);
        while (!cancellationToken.IsCancellationRequested && Scheduler.MoveNext())
        {
            var action = await Teams[MemberData[Scheduler.Current].Team].Provider.GetSelectionAsync(
                new PlayerActionRequest(
                    Scheduler.Current, GetEnvironment()), cancellationToken);

            var state = new LinkResolutionState(Scheduler.Current, [
                new ShardTargetingContext
                {
                    Target = action.MainTarget,
                    Rank = 0,
                }
            ], ModifierCollection.Empty, 0);
            var env = GetEnvironment();
            var builder = new CommandBuilder(_rng);
            foreach (var shard in action.Link)
            {
                shard.Resolve(ref state, env, builder);
                state = state with
                {
                    CurrentShardIndex = state.CurrentShardIndex + 1
                };
            }

            builder.Build(_commandResolver);
        }

        BattleEnded?.Invoke(this, EventArgs.Empty);
    }

    public BattleEnvironment GetEnvironment()
    {
        return new BattleEnvironment(MemberData, Teams);
    }

    public static Builder CreateBuilder()
    {
        return new Builder();
    }

    public class Builder
    {
        private readonly Dictionary<EntityUid, Fighter> _fighters = [];
        private readonly List<Team> _teams = [];

        internal Builder()
        {
        }

        public Builder AddTeam(ISelectionProvider provider)
        {
            _teams.Add(new Team(provider));
            return this;
        }

        public Builder AddTeams(IEnumerable<ISelectionProvider> teams)
        {
            foreach (var team in teams)
            {
                AddTeam(team);
            }

            return this;
        }

        public Builder AddMember(TeamUid alliedTeam, FighterDefinition member)
        {
            var data = new Fighter
            {
                Owner = member.Id,
                Team = alliedTeam,
            };
            _fighters[data.Id] = data;
            return this;
        }

        public Builder AddMembers(TeamUid alliedTeam, IEnumerable<FighterDefinition> members)
        {
            foreach (var member in members)
            {
                AddMember(alliedTeam, member);
            }

            return this;
        }

        public BattleManager Build()
        {
            return new BattleManager(_fighters.ToImmutableDictionary(), _teams.ToImmutableDictionary(t => t.Id));
        }
    }

    private class SeededBattleRng(int seed) : IBattleRng
    {
        private readonly Random _random = new(seed);

        public bool Chance(double chance)
        {
            return chance > _random.NextDouble();
        }

        public T Choose<T>(IEnumerable<T> values)
        {
            return values.MinBy(_ => _random.Next())
                   ?? throw new InvalidOperationException("Values cannot be an empty collection.");
        }

        public IEnumerable<T> Shuffle<T>(IEnumerable<T> source)
        {
            return source.OrderBy(_ => _random.Next());
        }
    }

    private class CommandResolver : ICommandVisitor
    {
        private readonly BattleManager _manager;

        public CommandResolver(BattleManager manager)
        {
            _manager = manager;
        }

        public void Visit(DamageCommand command)
        {
            var data = command.Data;
            var damage = command.TotalAmount;

            _manager.MemberData[command.Relation.Target].TakeDamage(damage, data);
        }

        public void Visit(MutateCommand command)
        {
            var data = command.Data;
            _manager.MemberData[command.Relation.Target].Pools[data.TargetPool].Value += command.TotalAmount;
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
            if (!_manager._rng.Chance(command.ApplicationChance))
            {
                _manager.EffectResisted?.Invoke(this, args);
                return;
            }

            _manager.MemberData[command.Relation.Target].AddStatusEffect(new StatusEffectInstance
            {
                Applicant = command.Relation.Actor,
                Holder = command.Relation.Target,
                Definition = data.Effect,
                Stacks = data.AddedStacks,
                Turns = data.InitialTurns
            });
            _manager.EffectAdded?.Invoke(this, args);
        }

        public void Visit(RemoveEffectCommand command)
        {
            var data = command.Data;
            foreach (var effect in _manager.MemberData[command.Relation.Target].StatusEffects.Where(data.Predicate))
            {
                if (effect.Definition.Properties.HasFlag(EffectFlags.Unremovable)) continue;

                if (effect.Definition.Properties.HasFlag(EffectFlags.RemoveStack))
                {
                    var eff = _manager.MemberData[command.Relation.Target].StatusEffects
                        .SingleOrDefault(sei => sei.Definition.Id == effect.Definition.Id);
                    if (eff != null) eff.Stacks -= 1;
                }
                else
                {
                    _manager.MemberData[command.Relation.Target].RemoveStatusEffect(effect.Definition.Id);
                }
            }
        }

        public void Visit(AdvanceTurnCommand command)
        {
            var data = command.Data;
            _manager.Scheduler.AdvanceEntity(command.Relation.Target, data.AdvancePercent);
        }

        public void Visit(QueueTurnCommand command)
        {
            var data = command.Data;
            _manager.Scheduler.Schedule(command.Relation.Target, data.BaseActionValue);
        }
    }
}

public interface IBattle
{
    Task RunAsync(CancellationToken cancellationToken);
}

public class CommandLaunchingArgs : EventArgs
{
    public required BattleCommand Command { get; init; }
}

public class ActionLaunchedArgs : EventArgs
{
    public required BattleCommand Command { get; init; }
}