using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.Calculation.Pools;
using FrogBattleV4.Core.Effects.StatusEffects;

namespace FrogBattleV4.Core.Calculation;

public class PoolContainer : IEnumerable<PoolComponent>
{
    private readonly Dictionary<PoolId, PoolComponent> _pools = new();
    private readonly List<IMutatorComponent> _mutators = [];

    [NotNull]
    public IEnumerable<IMutatorComponent> Mutators
    {
        get => _mutators;
        init => _mutators = value.ToList();
    }

    public void AddMutator(IMutatorComponent mutator) => _mutators.Add(mutator);
    public void TickStart()
    {
        foreach (var mc in Mutators)
        {
            mc.OnTurnStart(this);
        }
    }

    public void TickEnd()
    {
        foreach (var mc in Mutators)
        {
            mc.OnTurnEnd(this);
        }
    }

    public bool Add(PoolInitContext pool) => _pools.TryAdd(pool.Definition.Id, new PoolComponent(pool));

    public bool Remove(PoolId id) => _pools.Remove(id);

#nullable enable
    /// <summary>
    /// Dictionary-like indexing used to get pools by id.
    /// </summary>
    /// <param name="id">Key to search pools by.</param>
    public PoolComponent? this[PoolId id] => _pools.GetValueOrDefault(id);

    public IEnumerator<PoolComponent> GetEnumerator() => _pools.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}