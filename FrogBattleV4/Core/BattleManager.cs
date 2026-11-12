using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Registries;

namespace FrogBattleV4.Core;

public sealed partial class BattleManager
{
    private readonly SeededBattleRng _rng;
    private readonly CommandResolver _commandResolver;
    private readonly FighterRegistry _fighterRegistry;
    private readonly ShardRegistry _shardRegistry;
    private readonly StatusEffectRegistry _statusEffectRegistry;
    private CommandBuilder _commandBuilder;

    private BattleManager(
        ImmutableDictionary<EntityUid, EntityInstance> entities,
        ImmutableDictionary<TeamUid, TeamInstance> teams,
        FighterRegistry fighterRegistry,
        ShardRegistry shardRegistry,
        StatusEffectRegistry statusEffectRegistry)
    {
        CreationTime = DateTime.UtcNow.Ticks;
        _rng = new SeededBattleRng(CreationTime.GetHashCode());
        _commandResolver = new CommandResolver(this);
        _commandBuilder = new CommandBuilder();

        Entities = entities;
        Teams = teams;
        _fighterRegistry = fighterRegistry;
        _shardRegistry = shardRegistry;
        _statusEffectRegistry = statusEffectRegistry;
    }

    public ImmutableDictionary<EntityUid, EntityInstance> Entities { get; private set; }
    public ImmutableDictionary<TeamUid, TeamInstance> Teams { get; private set; }

    public long CreationTime { get; }
    private TurnScheduler Scheduler { get; } = new();

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        // TODO (Nova): incomplete
        foreach (var turn in GetScheduledActions(Entities.Keys))
        {
            Scheduler.Schedule(turn);
        }

        OnBattleStarted();
        IAction? previous = null;
        while (!cancellationToken.IsCancellationRequested && Scheduler.MoveNext())
        {
            var env = GetEnvironment();
            var action = Scheduler.Current;
            _commandBuilder = new CommandBuilder();

            // TODO (Nova): Status effect ticks should only happen on *scheduled* turns, not instant actions
            if (previous is not null) Entities[previous.Actor].Tick(TurnMoment.End, env, _commandBuilder);
            previous = action;
            // Tick current entity's start mutators
            Entities[action.Actor].Tick(TurnMoment.Start, env, _commandBuilder);

            // Mutator commands are executed separately
            _commandBuilder.Flush(_commandResolver);

            // Also check whether this entity was downed by DoT
            if (Entities[action.Actor].Downed) continue;

            OnTurnStarted(new TurnEventArgs
            {
                Actor = action.Actor,
            });

            var decision = await action.GetDecisionAsync(env, cancellationToken);

            var state = new LinkResolutionState(action.Actor, [
                new ShardTargetingContext
                {
                    Target = decision.MainTarget,
                    Rank = 0,
                },
            ], [], 0);

            // You can submit a shard link that contains unusable shards,
            // but if said shards don't become usable by the time
            // they need to be executed, the shard link stops.
            foreach (var shard in decision.Link.Select(_shardRegistry.Get).TakeWhile(s => s.IsUsable(state, env)))
            {
                shard.Resolve(ref state, env, _commandBuilder, _rng);
                state = state with
                {
                    CurrentShardIndex = state.CurrentShardIndex + 1,
                };
            }

            // Flush shard link commands
            _commandBuilder.Flush(_commandResolver);
            // Flush reaction commands from shard link resolution
            _commandBuilder.FlushClean(_commandResolver);

            // Check whether the battle should continue
            

            // Reschedule actions
            Scheduler.Schedule(new ScheduledTurn(action.Actor, 10000 / new StatQuery
            {
                Subject = action.Actor,
                Stat = StatName.Spd,
            }.Calculate(env), Teams[Entities[action.Actor].Team].Provider));
        }

        if (cancellationToken.IsCancellationRequested)
        {
            // Maybe some other potential logic for cancellation
            return;
        }

        OnBattleEnded();
    }

    private ScheduledTurn GetScheduledAction(EntityUid entity)
    {
        return new ScheduledTurn(entity, 10000 / new StatQuery
        {
            Subject = entity,
            Stat = StatName.Spd,
        }.Calculate(GetEnvironment()), Teams[Entities[entity].Team].Provider);
    }

    private IEnumerable<ScheduledTurn> GetScheduledActions(IEnumerable<EntityUid> entities)
    {
        // Small optimization to avoid getting a new environment for each entity
        var env = GetEnvironment();
        return entities.Select(entity => new ScheduledTurn(entity, 10000 / new StatQuery
        {
            Subject = entity,
            Stat = StatName.Spd,
        }.Calculate(env), Teams[Entities[entity].Team].Provider));
    }

    /// <summary>
    /// Returns a new battle environment populated with the current entities and teams, and no subjective modifiers.
    /// </summary>
    /// <returns>A new <see cref="BattleEnvironment"/> instance.</returns>
    public BattleEnvironment GetEnvironment()
    {
        return new BattleEnvironment(Entities, Teams, _fighterRegistry, _shardRegistry, [/*TODO*/]);
    }
}