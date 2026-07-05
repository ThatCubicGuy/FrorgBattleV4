using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Entities;

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