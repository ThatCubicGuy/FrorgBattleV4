namespace FrogBattleV4.Core.CharacterSystem;

public enum Stat
{
    None,
    MaxHp,
    MaxMana,
    MaxEnergy,          // Positive = Bad
    Atk,
    Def,
    Spd,
    Dex,
    CritRate,
    CritDamage,
    HitRateBonus,
    EffectHitRate,
    EffectRes,
    ManaCost,           // Positive = Bad
    ManaRegen,
    EnergyRecharge,
    IncomingHealing,
    OutgoingHealing,
    ShieldToughness,
}