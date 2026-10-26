using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4.Core;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Registries;
using FrogBattleV4.Core.Selections;
using FrogBattleV4.JsonDeserialization;

// using var game = new FrogBattleV4.FrogBattle();
// game.Run();

// TODO: Everything lmao
Console.WriteLine("Nothing here yet!");

var shardRegistry = ShardRegistry.Global;

shardRegistry.Replace(new ShardRegistry.Loader("frorgbattle").LoadAll());
Console.Out.WriteLine("Registered shards:");
foreach (var kvp in shardRegistry)
{
    Console.Out.WriteLine($"{kvp.Value.GetDetails()}");
    Console.Out.WriteLine("(end)");
}

var fighter = new Fighter
{
    Name = "Y/N",
    Shards = [.. shardRegistry.Select(kvp => kvp.Key)]
};

Console.Out.WriteLine(JsonSerializer.Serialize(fighter, AppSerializerContext.Default.Fighter));

var fighterRegistry = FighterRegistry.Global;

fighterRegistry.Replace(new FighterRegistry.Loader("frorgbattle").LoadAll());
Console.Out.WriteLine("Registered fighters:");
foreach (var kvp in fighterRegistry)
{
    Console.Out.WriteLine($"{kvp.Value.GetDetails()}");
    Console.Out.WriteLine("(end)");
}

// var console = new SetSelectionProvider();
// var member = new FighterId(new ResourceId("frorgbattle", "fighter1"));
// var manager = BattleManager.GetBuilder().AddTeam(console).AddMember(member).AddTeam(console).AddMember(member).Build();
// var task = manager.RunAsync(CancellationToken.None);
// Console.Out.WriteLine("Battle started");
// await task;
// Console.Out.WriteLine("Battle ended");

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
                Requirements: [{string.Join(", ", shard.Requirements.Select(req => req.GetDetails()))}]
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

file class SetSelectionProvider : ISelectionProvider
{
    public Task<PlayerTurnAction> GetSelectionAsync(PlayerActionRequest request, CancellationToken cancellationToken)
    {
        var shards = request.Environment
            .GetShards(request.User)
            .Select(ShardRegistry.Global.Get)
            .ToArray();
        var enemies = request.Environment
            .GetEnemies(request.User)
            .ToArray();
        return Task.FromResult(new PlayerTurnAction([.. shards], enemies[0]));
    }
}