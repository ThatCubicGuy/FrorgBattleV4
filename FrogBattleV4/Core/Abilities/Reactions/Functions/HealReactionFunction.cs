using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Abilities.Reactions.Functions;

public class HealReactionFunction : CommandReactionFunction
{
    public required double Ratio { get; init; }
    public bool CanOverheal { get; init; }

    protected override void HandleEvent(EntityUid holder, BattleEnvironment env, BattleCommandEventArgs args,
        CommandBuilder builder)
    {
        if (args is not IDeltaEventArgs delta) return;

        // TODO (Nova): actually publish reaction commands
        var cmd = GetRelation(holder, env, delta).Select(target => new HealCommand
        {
            Amount = delta.Delta * Ratio,
            Relation = target,
            Data = new HealData(CanOverheal),
        });
    }
}