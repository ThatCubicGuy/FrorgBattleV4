using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Calculation;

[JsonConverter(typeof(ExpandableEnumConverter<DamageForm>))]
public readonly record struct DamageForm(string Identifier) : IExpandableEnum
{
    public override string ToString() => Identifier;

    #region Common Forms

    public static readonly DamageForm Blunt = new(nameof(Blunt).ToSnakeCase());
    public static readonly DamageForm Slash = new(nameof(Slash).ToSnakeCase());
    public static readonly DamageForm Pierce = new(nameof(Pierce).ToSnakeCase());
    public static readonly DamageForm Bullet = new(nameof(Bullet).ToSnakeCase());
    public static readonly DamageForm Blast = new(nameof(Blast).ToSnakeCase());
    public static readonly DamageForm Magic = new(nameof(Magic).ToSnakeCase());

    #endregion

    /// <summary>
    /// Determines whether a modifier of this type can boost damage from type <paramref name="form"/>.
    /// </summary>
    /// <param name="form">Damage type.</param>
    /// <returns>True if a modifier of this type can boost damage from
    /// type <paramref name="form"/>, false otherwise.</returns>
    [System.Diagnostics.Contracts.Pure]
    public bool Permits(DamageForm form) => this == form || this == default;
}