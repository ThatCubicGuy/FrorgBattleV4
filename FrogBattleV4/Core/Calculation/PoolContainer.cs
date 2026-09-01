using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Abilities.Components.Actions;
using FrogBattleV4.Core.Calculation.Pools;
using FrogBattleV4.Core.Effects.StatusEffects;

namespace FrogBattleV4.Core.Calculation;

public class PoolContainer
{
    private readonly Dictionary<PoolId, PoolComponent> _pools = new();
    private readonly List<IMutatorComponent> _mutators = [];

    public void AddMutator(IMutatorComponent mutator) => _mutators.Add(mutator);

    public void TickStart()
    {
        foreach (var mc in _mutators)
        {
            mc.OnTurnStart(this);
        }
    }

    public void TickEnd()
    {
        foreach (var mc in _mutators)
        {
            mc.OnTurnEnd(this);
        }
    }

    public void TakeDamage(DealDamage damage)
    {
        var pool = LastWithTag(PoolTag.AbsorbsDamage) ??
                   LastWithTag(PoolTag.UsedForLife);
        if (pool is null)
        {
            System.Diagnostics.Trace.WriteLine("WARNING: Attempt to damage member with no health!");
            return;
        }

        pool.CurrentValue -= damage.TotalAmount;
    }

    public void Mutate(Mutate result)
    {
        if (_pools.GetValueOrDefault(result.TargetPool) is not { } pool)
        {
            System.Diagnostics.Debug.WriteLine("WARNING: Attempt to mutate absent pool in member!");
            return;
        }

        pool.CurrentValue += result.TotalAmount;
    }

    public bool Add(PoolInitContext pool) => _pools.TryAdd(pool.Definition.Id, new PoolComponent(pool));

    public bool Remove(PoolId id) => _pools.Remove(id);

    /// <summary>
    /// Dictionary-like indexing used to get pool values by id.
    /// </summary>
    /// <param name="id">Key to search pools by.</param>
    [Pure]
    public PoolSnapshot this[PoolId id] => _pools.GetValueOrDefault(id) is { } pool ? new PoolSnapshot(pool) : default;

    [Pure]
    private IEnumerable<PoolComponent> WithTag(PoolTag tag) => _pools.Values.Where(pc => pc.HasTag(tag));

    [Pure]
    private PoolComponent? LastWithTag(PoolTag tag) => _pools.Values.LastOrDefault(pc => pc.HasTag(tag));
}

public readonly record struct PoolSnapshot(double CurrentValue, double? MinValue, double? MaxValue)
{
    public PoolSnapshot(PoolComponent pool) : this(pool.CurrentValue, pool.MinValue, pool.MaxValue) { }
    public override string ToString()
    {
        return $"{CurrentValue}" + (MaxValue is null ? string.Empty : $"/{MaxValue}");
    }
}