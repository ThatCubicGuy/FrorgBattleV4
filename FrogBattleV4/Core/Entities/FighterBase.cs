using System.Collections.Frozen;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;

namespace FrogBattleV4.Core.Entities;

public abstract class FighterBase : IBattleMember
{
    protected FighterBase(string name, Team team)
    {
        Name = name;
        AlliedTeam = team;
    }
    public string Name { get; }
    public Team AlliedTeam { get; }
    public FrozenDictionary<StatId, double> BaseStats { get; } = Registry.BaseCharacterStats;
    public PoolContainer Pools { get; } = new();

    public double GetStat(StatQuery query)
    {
        return BaseStats[query.Stat];
    }

    public PoolSnapshot GetPool(PoolQuery query)
    {
        return Pools[query.Pool];
    }
}