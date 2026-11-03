using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Combat.Commands;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Modifiers.StatusEffects;
using FrogBattleV4.Core.Registries;
using FrogBattleV4.Core.Selections;

namespace FrogBattleV4.Core;

// TODO: Full revamp
public sealed partial class BattleManager
{
    private readonly SeededBattleRng _rng;
    private readonly CommandResolver _commandResolver;
    private readonly FighterRegistry _fighterRegistry;
    private readonly ShardRegistry _shardRegistry;
    private readonly StatusEffectRegistry _statusEffectRegistry;

    private BattleManager(
        ImmutableDictionary<EntityUid, Entity> entities,
        ImmutableDictionary<TeamUid, Team> teams,
        FighterRegistry fighterRegistry,
        ShardRegistry shardRegistry,
        StatusEffectRegistry statusEffectRegistry)
    {
        CreationTime = DateTime.UtcNow.Ticks;
        _rng = new SeededBattleRng(CreationTime.GetHashCode());
        _commandResolver = new CommandResolver(this);
        _fighterRegistry = fighterRegistry;
        _shardRegistry = shardRegistry;
        _statusEffectRegistry = statusEffectRegistry;
        Entities = entities;
        Teams = teams;
    }

    public ImmutableDictionary<EntityUid, Entity> Entities { get; private set; }
    public ImmutableDictionary<TeamUid, Team> Teams { get; private set; }

    public long CreationTime { get; }
    private TurnScheduler Scheduler { get; } = new();

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        // TODO (Nova): incomplete
        foreach (var uid in Entities.Keys)
        {
            Scheduler.Schedule(uid, 10000 / new StatQuery
            {
                Subject = uid,
                Stat = StatName.Spd,
            }.Calculate(GetEnvironment()));
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
                },
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
                    CurrentShardIndex = state.CurrentShardIndex + 1,
                };
            }

            builder.Build(_commandResolver);
        }

        OnBattleEnded();
    }

    /// <summary>
    /// Returns a new battle environment populated with the current entities and teams, and no subjective modifiers.
    /// </summary>
    /// <returns>A new <see cref="BattleEnvironment"/> instance.</returns>
    public BattleEnvironment GetEnvironment()
    {
        return new BattleEnvironment(Entities, Teams, _fighterRegistry, _shardRegistry);
    }
}