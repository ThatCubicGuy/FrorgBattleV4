using System.Collections.Frozen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core;

public class StatContainer([NotNull] IEnumerable<KeyValuePair<StatId, double>> stats) : IBattleMemberComponent
{
    private readonly FrozenDictionary<StatId, double> _stats = stats.ToFrozenDictionary();

    public double this[StatId id] => _stats.GetValueOrDefault(id);
}