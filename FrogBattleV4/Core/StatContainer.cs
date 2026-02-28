using System.Collections.Frozen;
using System.Collections.Generic;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core;

public class StatContainer(IDictionary<StatId, double> stats)
{
    private readonly FrozenDictionary<StatId, double> _stats = stats.ToFrozenDictionary();

    public double this[StatId id] => _stats.GetValueOrDefault(id);
}