using System.Collections.Frozen;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FrogBattleV4.Core.Abilities.Components.Actions;
using FrogBattleV4.Core.Calculation;
using FrogBattleV4.Core.Combat;
using FrogBattleV4.Core.Effects.Modifiers;
using FrogBattleV4.Core.Effects.StatusEffects;

namespace FrogBattleV4.Core.Entities;

public abstract class FighterBase(string name, Team team) : GameEntity,
    ITurnCycleMember,
    IModifierProvider
{
    private readonly List<IModifierProvider> _statusEffects = [];
    
    public string Name { get; } = name;
    public Team AlliedTeam { get; } = team;
    public FrozenDictionary<StatId, double> BaseStats { get; init; } = Registry.BaseCharacterStats;
    public PoolContainer Pools { get; } = new();
    public ReadOnlyCollection<IModifierProvider> StatusEffects => _statusEffects.AsReadOnly();

    public double GetStat(StatQuery query)
    {
        return BaseStats[query.Stat];
    }

    public Turn GetNextTurn()
    {
        throw new System.NotImplementedException();
    }

    public ModifierStack GetContributingModifiers(ModifierQuery query)
    {
        return _statusEffects.Aggregate(new ModifierStack(),
            (stack, modifier) => stack + modifier.GetContributingModifiers(query));
    }

    public void TakeDamage(DealDamage damage)
    {
        Pools.TakeDamage(damage);
    }

    public void Mutate(Mutate mutate)
    {
        Pools.Mutate(mutate);
    }

    public void ApplyEffect(ApplyEffect effect)
    {
        _statusEffects.Add(new StatusEffectInstance(effect));
    }
}