using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Combat.Commands;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Registries;
using FrogBattleV4.Core.Selections;

namespace FrogBattleV4.Core;

// TODO: Full revamp
public partial class BattleManager : IBattle
{
    private readonly SeededBattleRng _rng;
    private readonly CommandResolver _commandResolver;
    private readonly FighterRegistry _fighterRegistry = FighterRegistry.Global;
    private readonly ShardRegistry _shardRegistry = ShardRegistry.Global;

    public event EventHandler? BattleStarted;
    public event EventHandler? BattleEnded;

    private BattleManager(ImmutableDictionary<EntityUid, Entity> entities, ImmutableDictionary<TeamUid, Team> teams)
    {
        CreationTime = DateTime.UtcNow.Ticks;
        _rng = new SeededBattleRng(CreationTime.GetHashCode());
        _commandResolver = new CommandResolver(this);
        Entities = entities;
        Teams = teams;
    }

    public long CreationTime { get; private set; }
    public TurnScheduler Scheduler { get; } = new();

    public ImmutableDictionary<EntityUid, Entity> Entities { get; private set; }
    public ImmutableDictionary<TeamUid, Team> Teams { get; private set; }

    public static Builder GetBuilder()
    {
        return new Builder();
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        // TODO (Nova): incomplete
        OnBattleStarted();
        while (!cancellationToken.IsCancellationRequested && Scheduler.MoveNext())
        {
            var env = GetEnvironment();
            var actor = Scheduler.Current;

            var action = await Teams[Entities[actor].Team].Provider.GetSelectionAsync(
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

        OnBattleEnded();
    }

    public BattleEnvironment GetEnvironment()
    {
        return new BattleEnvironment(Entities, Teams, _fighterRegistry, _shardRegistry);
    }

    protected virtual void OnBattleStarted()
    {
        BattleStarted?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void OnBattleEnded()
    {
        BattleEnded?.Invoke(this, EventArgs.Empty);
    }

    public static Builder CreateBuilder()
    {
        return new Builder();
    }

    public class Builder
    {
        private readonly Dictionary<EntityUid, Entity> _fighters = [];
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

        public Builder AddMember(TeamUid alliedTeam, FighterId member)
        {
            var data = new Entity
            {
                Owner = member,
                Team = alliedTeam,
            };
            _fighters[data.Id] = data;
            return this;
        }

        public Builder AddMembers(TeamUid alliedTeam, IEnumerable<FighterId> members)
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
}

public interface IBattle
{
    Task RunAsync(CancellationToken cancellationToken);
}

public class CommandLaunchedEventArgs : EventArgs
{
    public required BattleCommand Command { get; init; }
}