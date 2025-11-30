using System.Collections.Generic;
using System.Linq;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.CharacterSystem.Components;
using FrogBattleV4.Core.EffectSystem;
using FrogBattleV4.Core.EffectSystem.Components;

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
        { nameof(Stats.MaxHp), 400000 },
        { nameof(Stats.MaxMana), 100 },
        { nameof(Stats.MaxEnergy), 120 },
        { nameof(Stats.Atk), 1000 },
        { nameof(Stats.Def), 500 },
        { nameof(Stats.Spd), 100 },
        { nameof(Stats.Dex), 0 },
        { nameof(Stats.CritRate), 0.1 },
        { nameof(Stats.CritDamage), 0.5 },
        { nameof(Stats.HitRateBonus), 0 },
        { nameof(Stats.EffectHitRate), 1 },
        { nameof(Stats.EffectRes), 0 },
        { nameof(Stats.ManaCost), 1 },
        { nameof(Stats.ManaRegen), 1 },
        { nameof(Stats.EnergyRecharge), 1 },
        { nameof(Stats.IncomingHealing), 1 },
        { nameof(Stats.OutgoingHealing), 1 },
        { nameof(Stats.ShieldToughness), 1 },
    };
    
    public List<AbilityDefinition> Abilities { get; } = [];
    public List<IEffect> ActiveEffects { get; } = [];
    public double GetStat(string stat)
    {
        return BaseStats[stat] + TODO;
    }

    public double GetStatVersus(string stat, ICharacter target)
    {
        return GetStat(stat) + TODO;
    }
}