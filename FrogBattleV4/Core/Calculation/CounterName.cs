using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Calculation;

[JsonConverter(typeof(ExpandableEnumConverter<CounterName>))]
public readonly record struct CounterName(string Identifier) : IExpandableEnum
{
    public override string ToString() => Identifier;

    #region Common counters

    public static readonly CounterName Barrier = new(nameof(Barrier).ToSnakeCase());

    #endregion
}