using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Calculation;

[JsonConverter(typeof(NameConverter<CounterName>))]
public readonly record struct CounterName(string Name) : IPropertyName
{
    public override string ToString() => Name;

    #region Common counters

    public static readonly CounterName Barrier = new(nameof(Barrier).ToSnakeCase());

    #endregion
}