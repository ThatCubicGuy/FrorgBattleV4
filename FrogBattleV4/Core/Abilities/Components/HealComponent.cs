using System.Text.Json.Serialization;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Abilities.Components;

public class HealComponent : ShardComponent
{
    public required AmountFormula Formula { get; init; }
    public required HealFlags Flags { get; init; }

    public override void GenerateCommands(ShardResolutionScope scope, BattleEnvironment env, CommandBuilder builder)
    {
        builder.Emit(new HealCommand
        {
            Amount = Formula.Resolve(scope, env),
            Flags = Flags,
            Relation = new RelationContext
            {
                Actor = scope.User,
                Target = scope.Targeting.Target,
            },
        });
    }
}