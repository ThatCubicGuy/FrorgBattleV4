using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.Calculation.Pools;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.Calculation;

public static class CommandPipeline
{
    /// <summary>
    /// Helper method that wraps other pipelines and executes the according method for any command.
    /// </summary>
    /// <param name="cmd">Command to execute.</param>
    /// <param name="ctx">Context in which to execute the command.</param>
    public static void ExecuteCommand(this IBattleCommand cmd, ModifierContext ctx)
    {
        switch (cmd)
        {
            case DamageCommand dc:
                dc.ExecuteDamage(ctx);
                break;
            case MutationCommand mc:
                mc.ExecuteMutation(ctx);
                break;
            default:
                throw new System.NotImplementedException();
        }
    }
}