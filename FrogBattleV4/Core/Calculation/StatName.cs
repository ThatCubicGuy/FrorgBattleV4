using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Calculation;

[JsonConverter(typeof(NameConverter<StatName>))]
public readonly record struct StatName(string Name) : IPropertyName
{
    public override string ToString() => Name;

    #region Common Stats

    public static readonly StatName MaxHp = new(nameof(MaxHp).ToSnakeCase());
    public static readonly StatName MaxMana = new(nameof(MaxMana).ToSnakeCase());
    public static readonly StatName MaxEnergy = new(nameof(MaxEnergy).ToSnakeCase());
    public static readonly StatName Atk = new(nameof(Atk).ToSnakeCase());
    public static readonly StatName Def = new(nameof(Def).ToSnakeCase());
    public static readonly StatName Spd = new(nameof(Spd).ToSnakeCase());
    public static readonly StatName Dex = new(nameof(Dex).ToSnakeCase());
    public static readonly StatName CritRate = new(nameof(CritRate).ToSnakeCase());
    public static readonly StatName CritDamage = new(nameof(CritDamage).ToSnakeCase());
    public static readonly StatName HitRateBonus = new(nameof(HitRateBonus).ToSnakeCase());
    public static readonly StatName EffectHitRate = new(nameof(EffectHitRate).ToSnakeCase());
    public static readonly StatName EffectRes = new(nameof(EffectRes).ToSnakeCase());
    public static readonly StatName ManaCost = new(nameof(ManaCost).ToSnakeCase());
    public static readonly StatName ManaRegen = new(nameof(ManaRegen).ToSnakeCase());
    public static readonly StatName EnergyRecharge = new(nameof(EnergyRecharge).ToSnakeCase());
    public static readonly StatName IncomingHealing = new(nameof(IncomingHealing).ToSnakeCase());
    public static readonly StatName OutgoingHealing = new(nameof(OutgoingHealing).ToSnakeCase());
    public static readonly StatName ShieldToughness = new(nameof(ShieldToughness).ToSnakeCase());

    #endregion
}