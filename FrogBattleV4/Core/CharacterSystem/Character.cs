using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.CharacterSystem.Components;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.ActiveEffects;

namespace FrogBattleV4.Core.CharacterSystem;

public class Character : ICharacter
{
    private Dictionary<string, IPoolComponent> _pools;

    public Character()
    {
        _pools = new Dictionary<string, IPoolComponent>
        {
            { "hp", new HealthComponent(this) },
            { "mana", new ManaComponent(this) },
            { "energy", new EnergyComponent(this) },
        };
    }
    
    // Base stats for any character
    public IReadOnlyDictionary<string, double> BaseStats { get; } = new Dictionary<string, double>()
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
    public List<IAttributeModifier> ActiveEffects { get; } = [];
    public List<IAttributeModifier> PassiveEffects { get; } = [];
    public double GetStat(string stat, ICharacter target = null)
    {
        return ActiveEffects.Aggregate(BaseStats[stat],
            (val, mod) => val + mod.GetModifiedStat(val, new StatContext
            {
                Stat = stat,
                EffectSource = (mod as ActiveEffectInstance)?.Source,
                Holder = this,
                Target = target,
            }));
    }
}