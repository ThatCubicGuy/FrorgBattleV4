using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Entities;

/// <summary>
/// Mutable in game data for any individual entity.
/// </summary>
public class Entity : Identifiable<EntityUid>
{
    private readonly List<IModifierProvider> _passiveModifiers = [];
    private readonly List<StatusEffectInstance> _statusEffects = [];
    private readonly Dictionary<PoolName, Pool> _pools = new();

    public event EventHandler<DamageTakenEventArgs>? DamageTaken;
    public event EventHandler<EffectAddedEventArgs>? EffectAdded;

    public Entity()
    {
        Pools = new ReadOnlyDictionary<PoolName, Pool>(_pools);
        StatusEffects = new ReadOnlyCollection<StatusEffectInstance>(_statusEffects);
    }

    /// <summary>
    /// ResourceId of the behavior definition for this entity.
    /// </summary>
    public required FighterId Owner { get; init; }
    public required TeamUid Team { get; init; }
    public ReadOnlyDictionary<PoolName, Pool> Pools { get; }
    public ReadOnlyCollection<StatusEffectInstance> StatusEffects { get; }
    public IEnumerable<IModifierProvider> GetAllModifiers() => _passiveModifiers.Concat(StatusEffects);

    /// <summary>
    /// Determines the order in which fighter pools absorb damage.
    /// </summary>
    private static readonly ImmutableArray<PoolName> DamagePriority =
    [
        PoolName.Shield,
        PoolName.Overheal,
        PoolName.Hp
    ];

    public void TakeDamage(double damage, DamageData damageData)
    {
        var args = new DamageTakenEventArgs
        {
            Amount = damage,
            Data = damageData
        };

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

        OnDamageTaken(args);
    }

    // TODO (Nova): don't like this way of implementing this functionality
    public void Tick(TurnMoment moment, Abilities.CommandBuilder builder)
    {
        foreach (var effect in StatusEffects)
        {
            effect.OnTick(moment, builder);
        }
    }

    public void AddStatusEffect(StatusEffectInstance effect)
    {
        _statusEffects.Add(effect);

        OnEffectAdded(new EffectAddedEventArgs
        {
            Definition = effect.Definition,
            Turns = effect.Turns,
            Stacks = effect.Stacks
        });
    }

    public void RemoveStatusEffect(StatusEffectId effectId)
    {

        _statusEffects.RemoveAll(sei => sei.ShouldRemove());
    }

    public void AddModifier(IModifierProvider modifier)
    {
        _passiveModifiers.Add(modifier);
    }

    protected virtual void OnDamageTaken(DamageTakenEventArgs e)
    {
        DamageTaken?.Invoke(this, e);
    }

    protected virtual void OnEffectAdded(EffectAddedEventArgs e)
    {
        EffectAdded?.Invoke(this, e);
    }
}

public class DamageTakenEventArgs : EventArgs
{
    public required double Amount { get; init; }
    public required DamageData Data { get; init; }
}

public class EffectAddedEventArgs
{
    public required StatusEffectDefinition Definition { get; init; }
    public required int Turns { get; init; }
    public required int Stacks { get; init; }
}