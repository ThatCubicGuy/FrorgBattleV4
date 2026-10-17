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

public class FighterData
{
    private readonly List<IModifierProvider> _passiveModifiers = [];
    private readonly List<StatusEffectInstance> _statusEffects = [];
    private readonly Dictionary<PoolId, PoolContainer> _pools = new();

    public event EventHandler<DamageTakenArgs>? TakingDamage;
    public event EventHandler<DamageTakenArgs>? TookDamage;
    public event EventHandler<EffectAddedArgs>? EffectAdded;

    public FighterData(Team alliedTeam)
    {
        Pools = new ReadOnlyDictionary<PoolId, PoolContainer>(_pools);
        StatusEffects = new ReadOnlyCollection<StatusEffectInstance>(_statusEffects);
    }
    public required EntityUid Owner { get; init; }
    public required TeamUid Team { get; init; }
    public required ReadOnlyDictionary<PoolId, PoolContainer> Pools { get; init; }
    public ReadOnlyCollection<StatusEffectInstance> StatusEffects { get; init; }
    public IEnumerable<IModifierProvider> GetAllModifiers() => _passiveModifiers.Concat(StatusEffects);

    // Order in which pools take damage
    [System.ComponentModel.ReadOnly(true)]
    private static readonly ImmutableArray<PoolId> DamagePriority =
    [
        PoolId.Shield,
        PoolId.Overheal,
        PoolId.Hp
    ];
    public void TakeDamage(double damage, DamageData damageData)
    {
        TakingDamage?.Invoke(this, new DamageTakenArgs { Amount = damage, Data = damageData });
        var poolId = DamagePriority.First(_pools.ContainsKey);
        Pools[poolId].Value -= damage;
        TookDamage?.Invoke(this, new DamageTakenArgs { Amount = damage, Data = damageData });
    }

    public void AddStatusEffect(StatusEffectInstance effect)
    {
        _statusEffects.Add(effect);
        EffectAdded?.Invoke(this, new EffectAddedArgs
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
}

public class DamageTakenArgs : EventArgs
{
    public required double Amount { get; init; }
    public required DamageData Data { get; init; }
}

public class EffectAddedArgs
{
    public required StatusEffectDefinition Definition { get; init; }
    public required int Turns { get; init; }
    public required int Stacks { get; init; }
}