using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Selections;

namespace FrogBattleV4.Core.Combat.Actions;

public class PeriodicSelectionTurn(IBattleMember user, ISelectionProvider provider) : IScheduledAction
{
    public IBattleMember Actor { get; } = user;
    public double BaseActionValue => 10000 / new ModifierContext(Actor).ComputeStat(StatId.Spd);

    public async Task<AbilityExecContext> PlayTurn(BattleContext ctx)
    {
        var defResult = await provider.GetSelectionAsync(
            new AbilitySelectionRequest(ctx.ActiveMember,
                ctx.ActiveMember.Abilities));
        var tgResult = await provider.GetSelectionAsync(
            new TargetSelectionRequest(ctx.ActiveMember,
                ctx.Enemies));
        return new AbilityExecContext
        {
            User = ctx.ActiveMember,
            Definition = defResult.Choices.Single(),
            MainTarget = tgResult.Choices.Single(),
            ValidTargets = ctx.Enemies,
            Rng = ctx.Rng,
        };
    }
}