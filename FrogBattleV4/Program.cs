using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Abilities.ShardClasses;
using FrogBattleV4.Core.Abilities.Targeting;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Registries;
using FrogBattleV4.Core.Selections;
using FrogBattleV4.JsonDeserialization;

// using var game = new FrogBattleV4.FrogBattle();
// game.Run();

// TODO: Everything lmao
Console.WriteLine("Nothing here yet!");

Console.Out.WriteLine(JsonSerializer.Serialize(
    new AffectShard
    {
        Name = "W", Description = "X", Function = ShardFunction.Attack,
        Targeting = new AreaOfEffectTargeting { RankPenalty = 1 },
        Components = []
    }, AppSerializerContext.Default.AffectShard));

var fighter = new Fighter
{
    Name = "Y/N",
    Shards = []
};

Console.Out.WriteLine(JsonSerializer.Serialize(fighter, AppSerializerContext.Default.Fighter));

var shardRegistry = RegistryLoader.CreateShards("frorgbattle").Load();

Console.Out.WriteLine("Registered shards:");
foreach (var kvp in shardRegistry)
{
    Console.Out.WriteLine($"{kvp.Value.GetDetails()}");
    Console.Out.WriteLine("(end)");
}

var fighterRegistry = RegistryLoader.CreateFighters("frorgbattle").Load();

Console.Out.WriteLine("Registered fighters:");
foreach (var kvp in fighterRegistry)
{
    Console.Out.WriteLine($"{kvp.Value.GetDetails()}");
    Console.Out.WriteLine("(end)");
}

var console = new SetSelectionProvider();
var member = new FighterId(new ResourceId("frorgbattle", "fighter1"));
var manager = BattleManager.GetBuilder().AddTeam(console).AddMember(member).AddTeam(console).AddMembers([member, member, member]).Build();
var task = manager.RunAsync(CancellationToken.None);
Console.Out.WriteLine("Battle started");
await task;
Console.Out.WriteLine("Battle ended");

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
}

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
            _ => -1
        };

        var rightPriority = right switch
        {
            TargetShard => 0,
            EmpowerShard => 1,
            AffectShard => 2,
            _ => -1
        };

        return leftPriority - rightPriority;
    }

    int IComparer<ShardId>.Compare(ShardId x, ShardId y) => OrderShards(x, y);
}