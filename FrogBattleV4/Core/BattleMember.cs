using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Combat.Actions;
using FrogBattleV4.Core.Combat.Selections;

namespace FrogBattleV4.Core;

public partial class BattleMember : IBattleMember
{
    private BattleMember(string name)
    {
        Name = name;
    }

    [NotNull] public string Name { get; }

    [NotNull] public required ITargetable Hitbox { get; init; }

    #region Containers

    public AbilityContainer Abilities { get; private init; }
    public StatContainer BaseStats { get; private init; }
    public EffectContainer Effects { get; private init; }
    public PoolContainer Pools { get; private init; }
    public TurnContainer Turn { get; private init; }

    #endregion
}

public class TurnContainer
{
    private readonly List<IScheduledAction> _actions = [];

    public async Task PlayTurn(ISelectionProvider provider, BattleContext ctx)
    {
        foreach (var action in _actions)
        {
            await action.PlayTurn(ctx);
        }
    }

    public void Add(IScheduledAction action) => _actions.Add(action);
}