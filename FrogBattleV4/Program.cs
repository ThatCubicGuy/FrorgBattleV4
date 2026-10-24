using System;
using System.Linq;
using System.Text.Json;
using FrogBattleV4.Core;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Abilities.Requirements;
using FrogBattleV4.Core.Abilities.ShardClasses;
using FrogBattleV4.Core.Abilities.Targeting;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.JsonDeserialization;

// using var game = new FrogBattleV4.FrogBattle();
// game.Run();

// TODO: Everything lmao
Console.WriteLine("Nothing here yet!");

var targetShard = new TargetShard
{
    Name = "Target Shard",
    Description = "Target shard hardcoded for testing.",
    Function = ShardFunction.Target,
    Requirements =
    [
        new StatRequirement
        {
            Stat = StatName.Atk,
            MinValue = 100,
        }
    ],
    Targeting = new BlastTargeting
    {
        MaximumRank = 5,
        Radius = 2,
        RankPenaltyPerSlot = 1
    }
};

var empowerShard = new EmpowerShard
{
    Name = "Empower Shard",
    Description = "Empower shard hardcoded for testing.",
    Function = ShardFunction.Enhance,
    Requirements = 
    [
        new PoolRequirement
        {
            Pool = PoolName.Mana,
            MinValue = 100,
        },
        new StatRequirement
        {
            Stat = StatName.Dex,
            MinValue = 20,
        }
    ],
    Modifiers = [
        new DamageModifier
        {
            AffectedSide = AffectedSide.This,
            CritOnly = false,
            Data = new DamageData(DamageType.Slash, DamageSource.Link),
            Direction = MutationDirection.Outgoing,
            ModifierStack = new ModifierStack
            {
                BaseAddValue = 10
            }
        }
    ]
};

var affectShard = new AffectShard
{
    Name = "Affect Shard",
    Description = "Affect shard hardcoded for testing.",
    Function = ShardFunction.Attack,
    CommandComponents =
    [
        new AttackComponent
        {
            Data = new DamageData(DamageType.Slash, DamageSource.Link),
            Formula = new DamageFormula.StatPercentage
            {
                Scalar = StatName.Atk,
                Ratio = 1.5,
                RankFalloff = 0.2
            },
        }
    ],
};

Console.Out.WriteLine(JsonSerializer.Serialize(targetShard, AppSerializerContext.Default.Shard));
Console.Out.WriteLine(JsonSerializer.Serialize(empowerShard, AppSerializerContext.Default.Shard));
Console.Out.WriteLine(JsonSerializer.Serialize(affectShard, AppSerializerContext.Default.Shard));

const string targetShardJson =
    """
    {
        "type": "target",
        "name": "Target Shard 2",
        "description": "Target shard hardcoded JSON.",
        "requirements": [
            {
                "type": "stat",
                "stat": "def",
                "min_value": 500
            }
        ],
        "targeting": {
           "type": "area_of_effect",
           "rank_penalty": 2
        },
        "function": 1
    }
    """;

const string empowerShardJson =
    """
    {
        "type": "target",
        "name": "Target Shard 2",
        "description": "Target shard hardcoded JSON.",
        "requirements": [
            {
                "type": "stat",
                "stat": "def",
                "min_value": 500
            }
        ],
        "targeting": {
           "type": "area_of_effect",
           "rank_penalty": 2
        },
        "function": 1
    }
    """;

var targetShardDeserialized = JsonSerializer.Deserialize(targetShardJson, AppSerializerContext.Default.Shard);
Console.Out.WriteLine(targetShardDeserialized is null
    ? "Whoops..."
    : targetShardDeserialized.ToDebugString());

file static class Extensions
{
    public static string ToDebugString(this Shard shard)
    {
        return $"""
                Type = {shard.GetType().Name}
                Name = {shard.Name}
                Description = {shard.Description}
                Function = {shard.Function}
                Requirements = {shard.Requirements.ToDebugString()}
                """;
    }

    public static string ToDebugString(this IShardRequirement requirement)
    {
        return $"Type = {requirement.GetType().Name}";
    }

    public static string ToDebugString(this IShardRequirement[] requirements)
    {
        return requirements.Aggregate("", (current, requirement) => $"{current}{requirement.ToDebugString()}");
    }
}