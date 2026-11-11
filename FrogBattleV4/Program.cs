using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FrogBattleV4;
using FrogBattleV4.Core;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Abilities.Components;
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
    Console.Out.WriteLine($"{kvp.GetDetails()}");
    Console.Out.WriteLine("(end)");
}

Console.Out.WriteLine("Registered fighters:");
foreach (var kvp in FighterRegistry.Global)
{
    Console.Out.WriteLine($"{kvp.GetDetails()}");
    Console.Out.WriteLine("(end)");
}

Console.Out.WriteLine("Registered effects:");
foreach (var kvp in StatusEffectRegistry.Global)
{
    Console.Out.WriteLine($"{kvp.Key}");
    Console.Out.WriteLine("(end)");
}

var provider = new SetDecisionProvider();
var member = new FighterId(new ResourceId("frorgbattle", "fighter_1"));
var member2 = new FighterId(new ResourceId("frorgbattle", "fighter_2"));
var manager = BattleManager.GetBuilder()
    .AddTeam(provider).AddMembers([member, member])
    .AddTeam(provider).AddMembers([member, member, member2])
    .Build();

manager.BattleStarted += Extensions.HandleBattleStarted;
manager.BattleEnded += Extensions.HandleBattleEnded;
manager.DamageTaken += Extensions.ReportDamageTaken;
manager.TurnStarted += Extensions.ReportStats;
manager.EffectApplied += (_, e) =>
{
    Console.Out.WriteLine($"{e.Stacks} stack(s) of {e.EffectId} applied for {e.Turns} turns by {e.Sender} to {e.Target}!");
};
manager.EffectResisted += (_, e) =>
{
    Console.Out.WriteLine($"{e.Target} resisted effect {e.EffectId} from {e.Sender}!");
};

try
{
    await manager.RunAsync(CancellationToken.None);
}
catch (BattleManager.EntityDownedException e)
{
    Console.Out.WriteLine($"Entity {e.Entity} was downed!");
}
catch (ApplicationException)
{
    Extensions.HandleBattleEnded(manager, EventArgs.Empty);
    Console.Out.WriteLine("Thank you for playing!");
}

Console.Out.WriteLine(Logger.GetLog());

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
    public static string GetDetails(this KeyValuePair<FighterId, Fighter> kvp)
    {
        return $"""
                Fighter "{kvp.Key}": [{string.Join(", ", kvp.Value.Shards)}]
                """;
    }

    public static string GetDetails(this KeyValuePair<ShardId, Shard> kvp)
    {
        return $"""
                {kvp.Value.GetType().Name} "{kvp.Key}" - {kvp.Key}
                Properties: {((kvp.Value as AffectShard)?.Components[0] as AttackComponent)?.Properties.CanCrit}
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

    public static void HandleBattleEnded(object? sender, EventArgs args)
    {
        foreach (var kvp in (sender as BattleManager)!.Entities)
        {
            Console.Out.WriteLine("{0} ({2}HP): {1}", kvp.Key, kvp.Value.ActiveEffects.Count, kvp.Value.Pools[PoolName.Hp].Value);
            foreach (var effect in kvp.Value.ActiveEffects)
            {
                Console.Out.WriteLine("effect.Turns = {0}", effect.Turns);
            }
        }
    }

    public static void ReportDamageTaken(object? sender, DamageTakenEventArgs e)
    {
        Console.Out.WriteLine($"{e.Amount:F0}{(e.IsCrit ? '!' : "")} {e.Metadata.Type} damage taken " +
                              $"by {e.Target} from {e.Sender}!");
    }

    private static int _turns;
    public static void ReportStats(object? sender, TurnEventArgs e)
    {
        _turns += 1;
        if (_turns > 20) throw new ApplicationException();
        var env = (sender as BattleManager)!.GetEnvironment();
        Console.Out.WriteLine(
            $"""
             Stats of {e.Actor}:
                 MaxHp = {new StatQuery
                 {
                     Stat = StatName.MaxHp,
                     Subject = e.Actor,
                 }.Calculate(env):F0}
                 Def = {new StatQuery
                 {
                     Stat = StatName.Def,
                     Subject = e.Actor,
                 }.Calculate(env):F0}
                 Spd = {new StatQuery
                 {
                     Stat = StatName.Spd,
                     Subject = e.Actor,
                 }.Calculate(env):F0}
                 CritRate = {new StatQuery
                 {
                     Stat = StatName.CritRate,
                     Subject = e.Actor,
                 }.Calculate(env):P0}
                 CritDamage = {new StatQuery
                 {
                     Stat = StatName.CritDamage,
                     Subject = e.Actor,
                 }.Calculate(env):P0}
             """);
    }
}