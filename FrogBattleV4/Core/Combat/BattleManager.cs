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
public partial class BattleManager : IBattle
{
    private readonly SeededBattleRng _rng;
    private readonly CommandResolver _commandResolver;

    public event EventHandler? BattleStarted;
    public event EventHandler? BattleEnded;

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
}

public interface IBattle
{
    Task RunAsync(CancellationToken cancellationToken);
}

public class CommandLaunchedEventArgs : EventArgs
{
    public required BattleCommand Command { get; init; }
}