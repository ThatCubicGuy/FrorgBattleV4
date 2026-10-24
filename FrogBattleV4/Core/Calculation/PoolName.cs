using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Calculation;

[JsonConverter(typeof(NameConverter<PoolName>))]
public readonly record struct PoolName(string Name) : IPropertyName
{
    public override string ToString() => Name;

    #region Common Pools

    public static readonly PoolName Hp = new(nameof(Hp).ToSnakeCase());
    public static readonly PoolName Mana = new(nameof(Mana).ToSnakeCase());
    public static readonly PoolName Energy = new(nameof(Energy).ToSnakeCase());
    public static readonly PoolName Shield = new(nameof(Shield).ToSnakeCase());
    public static readonly PoolName Overheal = new(nameof(Overheal).ToSnakeCase());

    #endregion
}