using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Calculation;

[JsonConverter(typeof(ExpandableEnumConverter<StatName>))]
public readonly record struct StatName(string Identifier) : IExpandableEnum
{
    public override string ToString() => Identifier;

    #region Common Stats

    public static readonly StatName MaxHp = new(nameof(MaxHp).ToSnakeCase());
    public static readonly StatName MaxEnergy = new(nameof(MaxEnergy).ToSnakeCase());
    public static readonly StatName Atk = new(nameof(Atk).ToSnakeCase());
    public static readonly StatName Def = new(nameof(Def).ToSnakeCase());
    public static readonly StatName Spd = new(nameof(Spd).ToSnakeCase());
    public static readonly StatName CritRate = new(nameof(CritRate).ToSnakeCase());
    public static readonly StatName CritDamage = new(nameof(CritDamage).ToSnakeCase());
    public static readonly StatName EffectHitRate = new(nameof(EffectHitRate).ToSnakeCase());
    public static readonly StatName EffectRes = new(nameof(EffectRes).ToSnakeCase());

    #endregion
}