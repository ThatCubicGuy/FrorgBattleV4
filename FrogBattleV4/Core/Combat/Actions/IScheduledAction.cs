using System.Linq;
using System.Threading.Tasks;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat.Selections;
using FrogBattleV4.Core.Entities;

namespace FrogBattleV4.Core.Combat.Actions;

public interface IScheduledAction : IBattleAction
{
    double BaseActionValue { get; }
}

public class CharacterTurn : IScheduledAction
{
    public required GameEntity Actor { get; init; }
    public required ISelectionProvider SelectionProvider { get; init; }

    public async Task<ShardLinkContext> PlayTurn(BattleContext ctx)
    {
        var defResult = await SelectionProvider.GetSelectionAsync(
            new AbilitySelectionRequest(ctx.ActiveMember,
                ctx.ActiveMember.Abilities));
        var tgResult = await SelectionProvider.GetSelectionAsync(
            new TargetSelectionRequest(ctx.ActiveMember,
                defResult.Choices.Single().TargetingPool switch
                {
                    TargetingPool.None => null,
                    TargetingPool.Allies => ctx.Allies,
                    TargetingPool.Enemies => ctx.Enemies,
                    TargetingPool.Self => [ctx.ActiveMember],
                    TargetingPool.Both => (ctx.Allies ?? []).Concat(ctx.Enemies ?? []),
                    TargetingPool.Arena => null,
                    _ => throw new System.NotSupportedException()
                }));
        return new ShardLinkContext
        {
            User = ctx.ActiveMember,
            CurrentLink = defResult.Choices.Single(),
            SelectedTarget = tgResult.Choices.Single(),
            Rng = ctx.Rng,
        };
    }

    public double BaseActionValue => 10000 / new InteractionContext(Actor).ComputeStat(StatId.Spd);
}

public interface IBattleAction
{
    GameEntity Actor { get; }
    Task<ShardLinkContext> PlayTurn(BattleContext ctx);
}