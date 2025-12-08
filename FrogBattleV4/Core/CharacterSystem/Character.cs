using System;
using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.CharacterSystem.Components;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;
using FrogBattleV4.Core.EffectSystem.Components;

namespace FrogBattleV4.Core.CharacterSystem;

public class Character : ICharacter
{
    private Dictionary<string, IPoolComponent> _pools;

    public Character()
    {
        _pools = new Dictionary<string, IPoolComponent>(StringComparer.OrdinalIgnoreCase)
        {
            {
                "hp", new HealthComponent(this)
                {
                    CurrentValue = BaseStats[nameof(Stat.MaxHp)]
                }
            },
            {
                "mana", new ManaComponent(this)
                {
                    CurrentValue = BaseStats[nameof(Stat.MaxMana)] / 2
                }
            },
            {
                "energy", new EnergyComponent(this)
                {
                    CurrentValue = 0
                }
            },
        };
    }
    
    // Base stats for any character
    public IReadOnlyDictionary<string, double> BaseStats { get; } =
        new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
    {
        { nameof(Stat.MaxHp), 400000 },
        { nameof(Stat.MaxMana), 100 },
        { nameof(Stat.MaxEnergy), 120 },
        { nameof(Stat.Atk), 1000 },
        { nameof(Stat.Def), 500 },
        { nameof(Stat.Spd), 100 },
        { nameof(Stat.Dex), 0 },
        { nameof(Stat.CritRate), 0.1 },
        { nameof(Stat.CritDamage), 0.5 },
        { nameof(Stat.HitRateBonus), 0 },
        { nameof(Stat.EffectHitRate), 1 },
        { nameof(Stat.EffectRes), 0 },
        { nameof(Stat.ManaCost), 1 },
        { nameof(Stat.ManaRegen), 1 },
        { nameof(Stat.EnergyRecharge), 1 },
        { nameof(Stat.IncomingHealing), 1 },
        { nameof(Stat.OutgoingHealing), 1 },
        { nameof(Stat.ShieldToughness), 1 },
    };
    
    public List<AbilityDefinition> Abilities { get; } = [];
    private List<IAttributeModifier> ActiveEffects { get; } = [];
    private List<IAttributeModifier> PassiveEffects { get; } = [];
    public List<IAttributeModifier> AttachedEffects => [..ActiveEffects, ..PassiveEffects];
    public double GetStat(string stat, ICharacter target = null)
    {
        return new StatContext
        {
            Stat = stat,
            Owner = this,
            Target = target
        }.ComputePipeline();
    }
}