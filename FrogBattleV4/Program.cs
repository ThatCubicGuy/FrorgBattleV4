using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Abilities.Reactions;
using FrogBattleV4.Core.Abilities.ShardClasses;
using FrogBattleV4.Core.Abilities.Targeting;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Registries;
using FrogBattleV4.Core.Selections;
using FrogBattleV4.JsonDeserialization;

// using var game = new FrogBattleV4.FrogBattle();
// game.Run();

// TODO: Everything lmao
Console.WriteLine("Nothing here yet!");

var elation = new CounterName("elation");

var counters = new Dictionary<CounterName, Counter>
{
    [elation] = new()
    {
        MaxValue = 20,
        DefaultValue = 0,
        TriggerValue = 3,
    },
}.ToImmutableDictionary();

var fighter = new Fighter
{
    Name = "Lunar Hero Lucine",
    Shards = [
        new ShardId(new ResourceId("frorgbattle", "affect_shard_1")),
        new ShardId(new ResourceId("frorgbattle", "target_shard_1")),
        new ShardId(new ResourceId("frorgbattle", "empower_shard_1")),
    ],
    Counters = counters,
    PassiveModifiers = [],
    Reactions = [
        new DirectDamageTakenReaction
        {
            Predicate = new DamageData(DamageType.All, DamageSource.Link),
            Functions = [
                new ModifyCounterReactionFunction
                {
                    Counter = elation,
                    Operation = ModifyCounterReactionFunction.CounterOperation.Increment,
                },
            ],
        },
    ],
};

Console.Out.WriteLine("Registered shards:");
foreach (var kvp in ShardRegistry.Global)
{
    Console.Out.WriteLine($"{kvp.Value.GetDetails()}");
    Console.Out.WriteLine("(end)");
}

Console.Out.WriteLine("Registered fighters:");
foreach (var kvp in FighterRegistry.Global)
{
    Console.Out.WriteLine($"{kvp.Value.GetDetails()}");
    Console.Out.WriteLine("(end)");
}

var provider = new SetSelectionProvider();
var member = new FighterId(new ResourceId("frorgbattle", "fighter1"));
var manager = BattleManager.GetBuilder().AddTeam(provider).AddMember(member).AddMember(member).AddTeam(provider).AddMembers([member, member, member]).Build();

manager.BattleStarted += Extensions.HandleBattleStarted;
manager.DamageTaken += (_, args) =>
{
    Console.Out.WriteLine($"{args.Amount} damage taken by {args.Target} from {args.Sender}!");
};

await manager.RunAsync(CancellationToken.None);

file class SetSelectionProvider : ISelectionProvider, IComparer<ShardId>
{
    public Task<PlayerTurnAction> GetSelectionAsync(PlayerActionRequest request, CancellationToken cancellationToken)
    {
        var shards = request.Environment
            .GetShards(request.User)
            .Order(this)
            .ToArray();
        var enemies = request.Environment
            .GetEnemies(request.User)
            .ToArray();
        return Task.FromResult(new PlayerTurnAction([.. shards], enemies[enemies.Length / 2]));
    }

    private static int OrderShards(ShardId a, ShardId b)
    {
        var left = ShardRegistry.Global.Get(a);
        var right = ShardRegistry.Global.Get(b);

        var leftPriority = left switch
        {
            TargetShard => 0,
            EmpowerShard => 1,
            AffectShard => 2,
            _ => -1,
        };

        var rightPriority = right switch
        {
            TargetShard => 0,
            EmpowerShard => 1,
            AffectShard => 2,
            _ => -1,
        };

        return leftPriority - rightPriority;
    }

    int IComparer<ShardId>.Compare(ShardId x, ShardId y) => OrderShards(x, y);
}

file static class Extensions
{
    public static string GetDetails(this Fighter fighter)
    {
        return $"""
                Fighter "{fighter.Name}": [{string.Join(", ", fighter.Shards)}]
                """;
    }

    public static string GetDetails(this Shard shard)
    {
        return $"""
                {shard.GetType().Name} "{shard.Name}" - {shard.Description}
                """;
    }

    public static string GetDetails(this IShardRequirement requirement)
    {
        return $"""
                {requirement.GetType().Name}
                """;
    }

    public static string GetDetails(this IShardCommandComponent shardCommand)
    {
        return $"""
                S
                """;
    }

    public static void HandleBattleStarted(object? sender, EventArgs args)
    {
        foreach (var kvp in (sender as BattleManager)!.Entities)
        {
            var counter = kvp.Value.Counters[new CounterName("elation")];

            counter.ValueChanged += (o, _) =>
            {
                if (o is not Counter ctr) return;

                Console.Out.WriteLine($"Value changed for counter of {kvp.Key} to {ctr.CurrentValue}!");
            };
            counter.TriggerValueReached += (_, _) =>
            {
                Console.Out.WriteLine($"Trigger value reached for entity {kvp.Key}!");
            };
        }
    }
}