using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Combat.Commands;
using FrogBattleV4.Core.Entities;
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
        foreach (var uid in Entities.Keys)
        {
            Scheduler.Schedule(uid, 100);
        }
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
            ], [], 0);

            var builder = new CommandBuilder();
            // You can submit a shard link that contains unusable shards,
            // but if said shards don't become usable by the time
            // they need to be executed, the shard link stops.
            foreach (var shard in action.Link.Select(_shardRegistry.Get).TakeWhile(s => s.IsUsable(state, env)))
            {
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

    public class Builder
    {
        private readonly Dictionary<EntityUid, Entity> _fighters = [];
        private readonly List<Team> _teams = [];

        internal Builder()
        {
        }

        /// <summary>
        /// Adds a new team to the builder with the given selection provider.
        /// </summary>
        /// <param name="provider">An object that will resolve selection requests for this team.</param>
        /// <returns>This builder instance.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="provider"/> is null.</exception>
        public Builder AddTeam(ISelectionProvider provider)
        {
            ArgumentNullException.ThrowIfNull(provider);
            if (_teams.Count > 0 && !_teams[^1].GetAllMembers().Any())
            {
                throw new InvalidOperationException("Cannot add a team when the previous team contains no members.");
            }

            _teams.Add(new Team(provider));
            return this;
        }

        /// <summary>
        /// Adds a member to the last added team of this builder.
        /// </summary>
        /// <param name="member">Member to add to the team.</param>
        /// <returns>This builder instance.</returns>
        /// <exception cref="InvalidOperationException">No team was added yet.</exception>
        public Builder AddMember(FighterId member)
        {
            if (_teams.Count == 0)
            {
                throw new InvalidOperationException("Cannot add members when there are no teams.");
            }

            var team = _teams[^1];
            var data = new Entity
            {
                Owner = member,
                Team = team.Uid,
            };
            _fighters[data.Uid] = data;
            team.AddMember(data.Uid);
            return this;
        }

        /// <summary>
        /// Adds multiple members to the last added team of this builder.
        /// </summary>
        /// <param name="members">Members to add to the team.</param>
        /// <returns>This builder instance.</returns>
        /// <exception cref="InvalidOperationException">No team was added yet.</exception>
        public Builder AddMembers(IEnumerable<FighterId> members)
        {
            if (_teams.Count == 0)
            {
                throw new InvalidOperationException("Cannot add members when there are no teams.");
            }

            foreach (var member in members)
            {
                AddMember(member);
            }

            return this;
        }

        public BattleManager Build()
        {
            if (_teams.Count < 2)
            {
                throw new InvalidOperationException("Cannot build a battle with less than two teams.");
            }

            return new BattleManager(_fighters.ToImmutableDictionary(), _teams.ToImmutableDictionary(t => t.Uid));
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