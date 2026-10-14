using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Modifiers;
using FrogBattleV4.Core.Modifiers.StatusEffects;

namespace FrogBattleV4.Core.Entities;

public class FighterData
{
    private readonly List<IModifierProvider> _passiveModifiers = [];
    private readonly List<StatusEffectInstance> _statusEffects = [];
    private readonly Dictionary<PoolId, PoolContainer> _pools = new();
    public FighterData()
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
    private static readonly PoolId[] DamagePriority =
    [
        PoolId.Shield,
        PoolId.Overheal,
        PoolId.Hp
    ];
    public void TakeDamage(double damage, DamageData damageData)
    {
        var poolId = DamagePriority.First(_pools.ContainsKey);
        Pools[poolId].Value -= damage;
    }
    public void AddStatusEffect(StatusEffectInstance effect)
    {
        _statusEffects.Add(effect);
    }

    public void RemoveStatusEffect(StatusEffectId effectId)
    {
        _statusEffects.RemoveAll(sei => sei.Definition.Id == effectId);
    }

    public void AddModifier(IModifierProvider modifier)
    {
        _passiveModifiers.Add(modifier);
    }
}