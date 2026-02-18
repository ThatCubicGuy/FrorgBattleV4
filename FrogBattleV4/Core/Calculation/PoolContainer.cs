#nullable enable
using System.Collections;
using System.Collections.Generic;
using FrogBattleV4.Core.Calculation.Pools;
using FrogBattleV4.Core.EffectSystem.StatusEffects;

namespace FrogBattleV4.Core.Calculation;

public class PoolContainer : IEnumerable<PoolComponent>
{
    private readonly Dictionary<PoolId, PoolComponent> _pools = new();

    public void TickStart()
    {
        foreach (var pool in _pools.Values)
        {
            foreach (var mc in pool.Mutators)
            {
                mc.OnTurnStart(pool);
            }
        }
    }

    public void TickEnd()
    {
        foreach (var pool in _pools.Values)
        {
            foreach (var mc in pool.Mutators)
            {
                mc.OnTurnEnd(pool);
            }
        }
    }

    public bool Add(PoolComponent pool) => _pools.TryAdd(pool.Id, pool);

    public bool Remove(PoolId id) => _pools.Remove(id);

    /// <summary>
    /// Dictionary-like indexing used to get pools by id.
    /// </summary>
    /// <param name="id">Key to search pools by.</param>
    public PoolComponent? this[PoolId id] => _pools.GetValueOrDefault(id);

    public IEnumerator<PoolComponent> GetEnumerator() => _pools.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

// not quite
public record PoolAddRequest(
    PoolId Id,
    IEnumerable<PoolTag> Tags,
    IEnumerable<IMutatorComponent> Mutators,
    double StartingValue = 0,
    double? MinValue = null,
    double? MaxValue = null)
{
    public PoolComponent Build()
    {
        return new PoolComponent
        {
            Id = Id,
            Tags = Tags,
            Mutators = Mutators,
            CurrentValue = StartingValue,
            MinValue = MinValue,
            MaxValue = MaxValue,
        };
    }
}