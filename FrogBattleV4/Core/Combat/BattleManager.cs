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
            var env = GetEnvironment();
            var actor = Scheduler.Current;

            var action = await Teams[MemberData[actor].Team].Provider.GetSelectionAsync(
                new PlayerActionRequest(actor, env), cancellationToken);

            var state = new LinkResolutionState(actor, [
                new ShardTargetingContext
                {
                    Target = action.MainTarget,
                    Rank = 0,
                }
            ], ModifierCollection.Empty, 0);

            var builder = new CommandBuilder();
            foreach (var shard in action.Link)
            {
                // You can submit a shard link that contains unusable shards,
                // but if said shards don't become usable by the time
                // they need to be executed, the shard link stops.
                if (!shard.IsUsable(state, env)) break;

                shard.Resolve(ref state, env, builder, _rng);
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
            var (actor, target) = command.Relation;
            var data = command.Data;

            _manager.MemberData[target]
                .TakeDamage(_manager._rng.Chance(command.CriticalChance)
                        ? command.CriticalAmount
                        : command.NormalAmount,
                    data);
        }

        public void Visit(MutateCommand command)
        {
            var (actor, target) = command.Relation;
            var data = command.Data;

            _manager.MemberData[target].Pools[data.TargetPool].Value += command.TotalAmount;
        }

        public void Visit(ApplyEffectCommand command)
        {
            var (actor, target) = command.Relation;
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

            _manager.MemberData[target].AddStatusEffect(new StatusEffectInstance
            {
                Applicant = actor,
                Holder = target,
                Definition = data.Effect,
                Stacks = data.AddedStacks,
                Turns = data.InitialTurns
            });
            _manager.EffectAdded?.Invoke(this, args);
        }

        public void Visit(RemoveEffectCommand command)
        {
            var (actor, target) = command.Relation;
            var data = command.Data;

            foreach (var effect in _manager.MemberData[target].StatusEffects.Where(data.Predicate))
            {
                if (effect.Definition.Properties.HasFlag(EffectFlags.Unremovable)) continue;

                if (effect.Definition.Properties.HasFlag(EffectFlags.RemoveStack))
                {
                    var eff = _manager.MemberData[target].StatusEffects
                        .SingleOrDefault(sei => sei.Definition.Id == effect.Definition.Id);
                    if (eff != null) eff.Stacks -= 1;
                }
                else
                {
                    _manager.MemberData[target].RemoveStatusEffect(effect.Definition.Id);
                }
            }
        }

        public void Visit(AdvanceTurnCommand command)
        {
            var (actor, target) = command.Relation;
            var data = command.Data;

            _manager.Scheduler.AdvanceEntity(target, data.AdvancePercent);
        }

        public void Visit(QueueTurnCommand command)
        {
            var (actor, target) = command.Relation;
            var data = command.Data;

            _manager.Scheduler.Schedule(target, data.BaseActionValue);
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