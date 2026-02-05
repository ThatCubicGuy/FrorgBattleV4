namespace FrogBattleV4.Core.CharacterSystem;

public struct StatQuery()
{
    public required string Stat { get; init; }
    public StatChannel Channel { get; init; } = StatChannel.Owned;
}

public enum StatChannel
{
    Owned,
    Penalty
}

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