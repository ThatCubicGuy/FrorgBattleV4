using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Calculation;

[JsonConverter(typeof(ExpandableEnumConverter<DamageType>))]
public readonly record struct DamageType(string Identifier) : IExpandableEnum
{
    public override string ToString() => Identifier;

    #region Common Types

    public static readonly DamageType All = default;
    public static readonly DamageType True = new(nameof(True).ToSnakeCase());
    public static readonly DamageType Blunt = new(nameof(Blunt).ToSnakeCase());
    public static readonly DamageType Slash = new(nameof(Slash).ToSnakeCase());
    public static readonly DamageType Pierce = new(nameof(Pierce).ToSnakeCase());
    public static readonly DamageType Bullet = new(nameof(Bullet).ToSnakeCase());
    public static readonly DamageType Blast = new(nameof(Blast).ToSnakeCase());
    public static readonly DamageType Magic = new(nameof(Magic).ToSnakeCase());

    #endregion

    /// <summary>
    /// Determines whether a modifier of this type can boost damage from type <paramref name="type"/>.
    /// </summary>
    /// <param name="type">Damage type.</param>
    /// <returns>True if a modifier of this type can boost damage from
    /// type <paramref name="type"/>, false otherwise.</returns>
    [System.Diagnostics.Contracts.Pure]
    public bool Permits(DamageType type) => this == type || this == All || type == All;
}