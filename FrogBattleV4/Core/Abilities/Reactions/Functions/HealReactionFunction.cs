using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Abilities.Reactions.Functions;

public class HealReactionFunction : CommandReactionFunction
{
    private readonly HealData _healData = HealData.None;
    public required double Ratio { get; init; }

    public bool CanOverheal
    {
        get => _healData.HasFlag(HealData.Overheals);
        init => _healData &= value ? HealData.Overheals : ~HealData.Overheals;
    }

    public bool CanRevive
    {
        get => _healData.HasFlag(HealData.Revives);
        init => _healData &= value ? HealData.Revives : ~HealData.Revives;
    }

    protected override void HandleEvent(EntityUid holder, BattleEnvironment env, BattleCommandEventArgs args,
        CommandBuilder builder)
    {
        if (args is not IDeltaEventArgs delta) return;

        var commands = GetRelation(holder, env, delta).Select(relation => new HealCommand
        {
            Amount = delta.Delta * Ratio,
            Relation = relation,
            Data = _healData,
        });

        foreach (var command in commands)
        {
            builder.Emit(command);
        }
    }
}