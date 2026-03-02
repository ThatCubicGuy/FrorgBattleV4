using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Selections;

namespace FrogBattleV4.Core.Combat.Actions;

public class SelectionTurn(IBattleMember user) : IScheduledAction
{
    public bool InstantRequeue => true;
    public IBattleMember Actor { get; } = user;
    public double BaseActionValue => 10000 / new ModifierContext(Actor).ComputeStat(StatId.Spd);
    public async Task PlayTurn(ISelectionProvider provider, BattleContext ctx)
    {
        var defResult = await provider.GetSelectionAsync(
            new AbilitySelectionRequest(ctx.ActiveMember,
                ctx.ActiveMember.Abilities));
        var tgResult = await provider.GetSelectionAsync(
            new TargetSelectionRequest(ctx.ActiveMember,
                ctx.Enemies));
        new AbilityExecContext
        {
            User = ctx.ActiveMember,
            Definition = defResult.Choices.Single(),
            MainTarget = tgResult.Choices.Single(),
            ValidTargets = ctx.Enemies,
            Rng = ctx.Rng,
        }.ExecuteAbility();
    }
}