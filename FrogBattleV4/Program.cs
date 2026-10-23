using System;
using System.Text.Json;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Abilities.Requirements;
using FrogBattleV4.Core.Abilities.ShardClasses;
using FrogBattleV4.Core.Abilities.Targeting;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.JsonDeserialization;

// using var game = new FrogBattleV4.FrogBattle();
// game.Run();

// TODO: Everything lmao
Console.WriteLine("Nothing here yet!");

var shard = new TargetShard
{
    Name = "Target Shard",
    Description = "Target shard hardcoded for testing.",
    Function = ShardFunction.Target,
    Requirement = new StatRequirement
    {
        Stat = StatName.Atk,
        MinValue = 100,
    },
    Targeting = new BlastTargeting
    {
        MaximumRank = 5,
        Radius = 2,
        RankPenaltyPerSlot = 1
    }
};

Console.Out.WriteLine(JsonSerializer.Serialize(shard, AppSerializerContext.Default.Shard));

const string jsonString =
    """
    {
        "type": "target",
        "name": "Target Shard 2",
        "description": "Target shard hardcoded JSON.",
        "requirement": {
            "type": "stat",
            "stat": {
                "id": "def"
            },
            "min_value": 500
        },
        "targeting": {
           "type": "area_of_effect",
           "rank_penalty": 2
        },
        "function": 1
    }
    """;

var deserializedShard = JsonSerializer.Deserialize(jsonString, AppSerializerContext.Default.Shard);
Console.Out.WriteLine(deserializedShard is null
    ? "Whoops..."
    : deserializedShard.ToDebugString());

file static class Extensions
{
    public static string ToDebugString(this Shard shard)
    {
        return $"""
                Type = {shard.GetType().Name}
                Name = {shard.Name}
                Description = {shard.Description}
                Function = {shard.Function}
                Requirement = {shard.Requirement.ToDebugString()}
                """;
    }

    public static string ToDebugString(this IShardRequirement requirement)
    {
        return $"Type = {requirement.GetType().Name}";
    }
}