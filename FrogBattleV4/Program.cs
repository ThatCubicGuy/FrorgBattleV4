using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4;
using FrogBattleV4.Core;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Abilities.ShardClasses;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Entities;
using FrogBattleV4.Core.Registries;
using FrogBattleV4.Core.Decisions;

// using var game = new FrogBattleV4.FrogBattle();
// game.Run();

// TODO: Everything lmao
Console.WriteLine("Nothing here yet!");

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

Console.Out.WriteLine("Registered effects:");
foreach (var kvp in StatusEffectRegistry.Global)
{
    Console.Out.WriteLine($"{kvp.Value.DisplayName}");
    Console.Out.WriteLine("(end)");
}

var provider = new SetDecisionProvider();
var member = new FighterId(new ResourceId("frorgbattle", "fighter1"));
var manager = BattleManager.GetBuilder()
    .AddTeam(provider).AddMembers([member, member])
    .AddTeam(provider).AddMembers([member])
    .Build();

manager.BattleStarted += Extensions.HandleBattleStarted;
manager.TurnStarted += (sender, e) =>
{
    var env = (sender as BattleManager)!.GetEnvironment();
    Console.Out.WriteLine($"""
                           Stats of {e.Actor}:
                               Def = {new StatQuery
                               {
                                   Stat = StatName.Def,
                                   Subject = e.Actor,
                               }.Calculate(env)}
                           """);
};
manager.EffectApplied += (_, e) =>
{
    Logger.WriteLine($"{e.Stacks} stack(s) of {e.EffectId} applied for {e.Turns} turns by {e.Sender} to {e.Target}!");
};
manager.EffectResisted += (_, e) =>
{
    Logger.WriteLine($"{e.Target} resisted effect {e.EffectId} from {e.Sender}!");
};

try
{
    await manager.RunAsync(CancellationToken.None);
}
catch (ApplicationException)
{
    Console.Out.WriteLine("Thank you for playing!");
}

file class SetDecisionProvider : IDecisionProvider, IComparer<ShardId>
{
    private static readonly Random Rng = new();
    public Task<TurnDecision> GetSelectionAsync(PlayerDecisionRequest request, CancellationToken cancellationToken)
    {
        var shards = request.Environment
            .GetShards(request.User)
            .OrderBy(GetShardPriority)
            .ToArray();
        var enemies = request.Environment
            .GetEnemies(request.User)
            .ToArray();
        Randomize(shards);
        return Task.FromResult(new TurnDecision([.. shards], enemies[enemies.Length / 2]));
    }

    private static int GetShardPriority(ShardId shard)
    {
        return ShardRegistry.Global.Get(shard) switch
        {
            TargetShard => 0,
            EmpowerShard => 1,
            AffectShard => 2,
            _ => -1,
        };
    }

    private static void Randomize(ShardId[] shards)
    {
        var idx = Array.FindIndex(shards, shard => ShardRegistry.Global.Get(shard) is AffectShard);
        var span = new Span<ShardId>(shards, idx, shards.Length - idx);
        Rng.Shuffle(span);
    }

    int IComparer<ShardId>.Compare(ShardId x, ShardId y) => GetShardPriority(x) - GetShardPriority(y);
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

    public static void HandleBattleStarted(object? sender, EventArgs args)
    {
        return;
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