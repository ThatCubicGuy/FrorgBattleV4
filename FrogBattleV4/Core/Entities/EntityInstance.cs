using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Modifiers.PassiveEffects;
using FrogBattleV4.Core.Modifiers.PassiveEffects.Conditions;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Entities;

/// <summary>
/// Mutable in game data for any individual entity.
/// </summary>
public class EntityInstance(Fighter definition, Team team) : UniquelyIdentifiable<EntityUid>
{
    /// <summary>
    /// Determines the order in which fighter pools absorb damage.
    /// </summary>
    private static readonly ImmutableArray<PoolName> DamagePriority =
    [
        PoolName.Shield,
        PoolName.Overheal,
        PoolName.Hp,
    ];

    private readonly Dictionary<StatusEffectId, StatusEffectInstance> _statusEffects = [];

    /// <summary>
    /// ResourceId of the behavior definition for this entity.
    /// </summary>
    public required FighterId Owner { get; init; }

    public required TeamUid Team { get; init; }

    public ImmutableDictionary<PoolName, Pool> Pools { get; } = BasePools(definition, team);
    public ImmutableDictionary<CounterName, Counter> Counters { get; } = BaseCounters(definition, team);
    public ImmutableList<PassiveModifier> PassiveModifiers { get; } = BasePassives(definition, team);

    public IReadOnlyCollection<StatusEffectInstance> StatusEffects => _statusEffects.Values;

    public void TakeDamage(double damage, DamageData damageData)
    {
        // Barriers block all damage taken in one instance
        if (Counters[CounterName.Barrier].TryDecrement()) return;

        // Go down the line of damage-taking pools
        foreach (var poolId in DamagePriority)
        {
            // "Consume" damage on the current pool
            damage -= Pools[poolId].Value;

            // If damage was less than the pool value, we must stop
            if (damage <= 0)
            {
                // Pool value becomes -(damage - oldValue) = oldValue - damage
                Pools[poolId].Value = -1 * damage;

                break;
            }

            // If damage was greater than the pool value, we keep going
            Pools[poolId].Value = 0;
        }
    }

    public void TakeHealing(double healing, HealData healData)
    {
        var hpPool = Pools[PoolName.Hp];

        // Calculate how much we can saturate the HP pool
        var maxHealing = hpPool.Capacity - hpPool.Value;

        // If we overflow max HP and this healing supports Overheal, add to the Overheal pool
        if (healing > maxHealing && healData.Overheals)
        {
            Pools[PoolName.Overheal].Value += healing - maxHealing;
        }

        // Add to HP pool irrespective of the above conditions.
        hpPool.Value += healing;
    }

    // TODO (Nova): don't like this way of implementing this functionality
    public void Tick(TurnMoment moment, Abilities.CommandBuilder builder)
    {
        foreach (var effect in StatusEffects)
        {
            effect.OnTick(moment, builder);
        }
    }

    public bool TryGetStatusEffect(StatusEffectId id, [MaybeNullWhen(false)] out StatusEffectInstance instance) =>
        _statusEffects.TryGetValue(id, out instance);

    public void AddStatusEffect(StatusEffectInstance effect)
    {
        var id = effect.Id;
        if (_statusEffects.TryGetValue(id, out var instance))
        {
            instance.Stacks += effect.Stacks;
            instance.Turns = effect.Turns;
            return;
        }

        _statusEffects[id] = effect;
    }

    public bool RemoveStatusEffect(StatusEffectId effect) => _statusEffects.Remove(effect);

    /// <summary>
    /// Returns a new set of default pools (Hp, Energy, Shield, Overheal)
    /// with the pools of the given definition and team appended.
    /// </summary>
    /// <param name="definition">Definition to get the pools from.</param>
    /// <param name="team">Team that this entity is a part of. Needed for common pools.</param>
    /// <returns>An ImmutableDictionary containing a new set of pools.</returns>
    private static ImmutableDictionary<PoolName, Pool> BasePools(Fighter definition, Team team)
    {
        var maxHp = definition.Stats[StatName.MaxHp];
        var maxEnergy = definition.Stats[StatName.MaxEnergy];
        var result = new Dictionary<PoolName, Pool>
        {
            [PoolName.Hp] = new(maxHp)
            {
                Value = maxHp,
            },
            [PoolName.Energy] = new(maxEnergy),
            [PoolName.Shield] = new(maxHp),
            [PoolName.Overheal] = new(maxHp),
        };

        foreach (var kvp in team.GetCommonPools())
        {
            result.Add(kvp.Key, kvp.Value);
        }

        foreach (var kvp in definition.Pools)
        {
            result.Add(kvp.Key, new Pool(kvp.Value.Capacity));
        }

        return result.ToImmutableDictionary();
    }

    /// <summary>
    /// Returns a new set of default counters (Barrier)
    /// with the counters of the given definition and team appended.
    /// </summary>
    /// <param name="definition">Definition to get the counters from.</param>
    /// <param name="team">Team that this entity is a part of. Needed for common counters.</param>
    /// <returns>An ImmutableDictionary containing a new set of counters.</returns>
    private static ImmutableDictionary<CounterName, Counter> BaseCounters(Fighter definition, Team team)
    {
        var result = new Dictionary<CounterName, Counter>
        {
            [CounterName.Barrier] = new(),
        };

        foreach (var kvp in team.GetCommonCounters())
        {
            result.Add(kvp.Key, kvp.Value);
        }

        foreach (var kvp in definition.Counters)
        {
            result.Add(kvp.Key, new Counter(kvp.Value));
        }

        return result.ToImmutableDictionary();
    }

    private static ImmutableList<PassiveModifier> BasePassives(Fighter definition, Team team)
    {
        var result = new List<PassiveModifier>(definition.Stats.Count + definition.PassiveModifiers.Length);
Logger.WriteLine(definition.Stats.Count.ToString());
        result.AddRange(definition.Stats.Select(kvp => new PassiveModifier
        {
            Modifiers =
            [
                new StatModifier
                {
                    Stat = kvp.Key, ModifierStack = new ModifierStack(kvp.Value),
                },
            ],
            Conditions = ConditionCollection.Empty,
        }));
        result.AddRange(definition.PassiveModifiers);

        return result.ToImmutableList();
    }
}