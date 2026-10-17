namespace FrogBattleV4.Core.Calculation;

public readonly record struct StatId(string Id)
{
    public override string ToString() => Id;

    #region Common Stats

    public static readonly StatId MaxHp = new(nameof(MaxHp).ToSnakeCase());
    public static readonly StatId MaxMana = new(nameof(MaxMana).ToSnakeCase());
    public static readonly StatId MaxEnergy = new(nameof(MaxEnergy).ToSnakeCase());
    public static readonly StatId Atk = new(nameof(Atk).ToSnakeCase());
    public static readonly StatId Def = new(nameof(Def).ToSnakeCase());
    public static readonly StatId Spd = new(nameof(Spd).ToSnakeCase());
    public static readonly StatId Dex = new(nameof(Dex).ToSnakeCase());
    public static readonly StatId CritRate = new(nameof(CritRate).ToSnakeCase());
    public static readonly StatId CritDamage = new(nameof(CritDamage).ToSnakeCase());
    public static readonly StatId HitRateBonus = new(nameof(HitRateBonus).ToSnakeCase());
    public static readonly StatId EffectHitRate = new(nameof(EffectHitRate).ToSnakeCase());
    public static readonly StatId EffectRes = new(nameof(EffectRes).ToSnakeCase());
    public static readonly StatId ManaCost = new(nameof(ManaCost).ToSnakeCase());
    public static readonly StatId ManaRegen = new(nameof(ManaRegen).ToSnakeCase());
    public static readonly StatId EnergyRecharge = new(nameof(EnergyRecharge).ToSnakeCase());
    public static readonly StatId IncomingHealing = new(nameof(IncomingHealing).ToSnakeCase());
    public static readonly StatId OutgoingHealing = new(nameof(OutgoingHealing).ToSnakeCase());
    public static readonly StatId ShieldToughness = new(nameof(ShieldToughness).ToSnakeCase());

    #endregion
}