using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Commands;

namespace FrogBattleV4.Core.Abilities.Reactions.Functions;

public class HealReactionFunction : CommandReactionFunction
{
    private readonly HealFlags _healFlags = HealFlags.None;
    public required double Ratio { get; init; }

    public bool CanOverheal
    {
        get => _healFlags.HasFlag(HealFlags.Overheals);
        init => _healFlags &= value ? HealFlags.Overheals : ~HealFlags.Overheals;
    }

    public bool CanRevive
    {
        get => _healFlags.HasFlag(HealFlags.Revives);
        init => _healFlags &= value ? HealFlags.Revives : ~HealFlags.Revives;
    }

    protected override void HandleEvent(EntityUid holder, BattleEnvironment env, BattleCommandEventArgs args,
        CommandBuilder builder)
    {
        if (args is not IDeltaEventArgs delta) return;

        var commands = GetRelation(holder, env, delta).Select(relation => new HealCommand
        {
            Amount = delta.Delta * Ratio,
            Relation = relation,
            Flags = _healFlags,
        });

        foreach (var command in commands)
        {
            builder.Emit(command);
        }
    }
}