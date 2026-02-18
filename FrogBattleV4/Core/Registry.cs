using System.Collections.Frozen;
using System.Collections.Generic;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core;

public static class Registry
{
    public static readonly FrozenDictionary<StatId, double> BaseCharacterStats = new Dictionary<StatId, double>
    {
        { StatId.MaxHp, 400000 },
        { StatId.MaxMana, 100 },
        { StatId.MaxEnergy, 120 },
        { StatId.Atk, 1000 },
        { StatId.Def, 500 },
        { StatId.Spd, 100 },
        { StatId.Dex, 0 },
        { StatId.CritRate, 0.1 },
        { StatId.CritDamage, 0.5 },
        { StatId.HitRateBonus, 0 },
        { StatId.EffectHitRate, 1 },
        { StatId.EffectRes, 0 },
        { StatId.ManaCost, 1 },
        { StatId.ManaRegen, 1 },
        { StatId.EnergyRecharge, 1 },
        { StatId.IncomingHealing, 1 },
        { StatId.OutgoingHealing, 1 },
        { StatId.ShieldToughness, 1 },
    }.ToFrozenDictionary();
}