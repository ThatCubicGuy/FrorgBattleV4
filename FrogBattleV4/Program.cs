using System;
using FrogBattleV4.Core.Registries;

// using var game = new FrogBattleV4.FrogBattle();
// game.Run();

// TODO: Everything lmao
Console.WriteLine("Nothing here yet!");

ShardRegistry.Global.Replace(new ShardRegistry.Loader("frorgbattle").LoadAll());

Console.Out.WriteLine("Registered shards:");
foreach (var kvp in ShardRegistry.Global)
{
    Console.WriteLine($"{kvp.Key}: {kvp.Value.Name} ({kvp.Value.Description})");
}