using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrogBattleV4.Core.Abilities;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Modifiers.PassiveEffects;
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

    private readonly Dictionary<StatusEffectId, StatusEffectInstance> _activeEffects = [];

    /// <summary>
    /// ResourceId of the behavior definition for this entity.
    /// </summary>
    public required FighterId Owner { get; init; }

    public required TeamUid Team { get; init; }

    public bool Downed { get; private set; }

    public ImmutableDictionary<PoolName, Pool> Pools { get; } = BasePools(definition, team);
    public ImmutableDictionary<CounterName, Counter> Counters { get; } = BaseCounters(definition, team);
    public ImmutableList<PassiveModifier> PassiveModifiers { get; } = BasePassives(definition, team);

    public IReadOnlyCollection<StatusEffectInstance> ActiveEffects => _activeEffects.Values;

    public void TakeDamage(double damage, DamageData data)
    {
        // Barriers block all damage taken in one instance
        if (data.HasFlag(DamageData.IgnoresBarrier) && Counters[CounterName.Barrier].TryDecrement()) return;

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

        // If we didn't survive the attack, mark this entity as downed
        if (Pools[PoolName.Hp].Value <= 0)
        {
            Downed = true;
        }
    }

    public void TakeHealing(double healing, HealData healData)
    {
        // Check if we can be revived
        if (Downed && !healData.HasFlag(HealData.Revives)) return;

        var hpPool = Pools[PoolName.Hp];

        // Calculate how much we can saturate the HP pool
        var maxHealing = hpPool.Capacity - hpPool.Value;

        // If we overflow max HP and this healing supports Overheal, add to the Overheal pool
        if (healing > maxHealing && healData.HasFlag(HealData.Overheals))
        {
            Pools[PoolName.Overheal].Value += healing - maxHealing;
        }

        // Add to HP pool irrespective of the above conditions.
        hpPool.Value += healing;

        // Set downed to false if we're above 0 hp
        if (Downed && hpPool.Value > 0)
        {
            Downed = false;
        }
    }

    // TODO (Nova): don't like this way of implementing this functionality
    public void Tick(TurnMoment moment, BattleEnvironment env, CommandBuilder builder)
    {
        var markedForDeath = new List<StatusEffectId>();

        foreach (var effect in ActiveEffects)
        {
            effect.OnTick(moment, env, builder);
            if (effect.ShouldRemove()) markedForDeath.Add(effect.Id);
        }

        foreach (var effect in markedForDeath)
        {
            _activeEffects.Remove(effect);
        }
    }

    public bool TryGetStatusEffect(StatusEffectId id, [MaybeNullWhen(false)] out StatusEffectInstance instance) =>
        _activeEffects.TryGetValue(id, out instance);

    public void AddStatusEffect(StatusEffectInstance effect)
    {
        var id = effect.Id;
        if (_activeEffects.TryGetValue(id, out var instance))
        {
            instance.Stacks += effect.Stacks;
            instance.Turns = effect.Turns;
            return;
        }

        _activeEffects[id] = effect;
    }

    public bool RemoveStatusEffect(StatusEffectId effect) => _activeEffects.Remove(effect);

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

        result.AddRange(definition.Stats.Select(kvp => new PassiveModifier
        {
            Modifiers =
            [
                new StatModifier
                {
                    Stat = kvp.Key,
                    // Create a new BaseAddValue modifier stack for each stat
                    ModifierValues = new ModifierStack(kvp.Value),
                },
            ],
            Conditions = [],
        }));
        result.AddRange(definition.PassiveModifiers);

        return result.ToImmutableList();
    }
}