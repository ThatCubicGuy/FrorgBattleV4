using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Combat.Actions;

namespace FrogBattleV4.Core;

public partial class BattleMember : IBattleMember
{
    private BattleMember(string name)
    {
        Name = name;
        Components.ComponentAdded += RegisterCache;
    }

    [NotNull] public string Name { get; }
    [NotNull] public ComponentContainer Components { get; } = [];

    [NotNull] public required ITargetable Hitbox { get; init; }
    [NotNull] public required IEnumerable<IScheduledAction> Turns { get; init; }

    #region Caches

    public AbilityContainer Abilities { get; private set; }
    public EffectContainer Effects { get; private set; }
    public PoolContainer Pools { get; private set; }
    public StatContainer BaseStats { get; private set; }

    internal void BuildCaches()
    {
        Abilities = (AbilityContainer)Components.FirstOrDefault(bmc => bmc is AbilityContainer);
        Effects = (EffectContainer)Components.FirstOrDefault(bmc => bmc is EffectContainer);
        Pools = (PoolContainer)Components.FirstOrDefault(bmc => bmc is PoolContainer);
        BaseStats = (StatContainer)Components.FirstOrDefault(bmc => bmc is StatContainer);
    }

    private void RegisterCache(IBattleMemberComponent component)
    {
        switch (component)
        {
            case AbilityContainer container:
                Abilities = container;
                break;
            case EffectContainer container:
                Effects = container;
                break;
            case PoolContainer container:
                Pools = container;
                break;
            case StatContainer container:
                BaseStats = container;
                break;
        }
    }

    #endregion
}