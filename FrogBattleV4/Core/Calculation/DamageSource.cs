using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Calculation;

[JsonConverter(typeof(ExpandableEnumConverter<DamageSource>))]
public readonly record struct DamageSource(string Identifier) : IExpandableEnum
{
    public override string ToString() => Identifier;

    #region Common Sources

    public static readonly DamageSource Link = new(nameof(Link).ToSnakeCase());
    public static readonly DamageSource FollowUp = new(nameof(FollowUp).ToSnakeCase());
    public static readonly DamageSource SoulLink = new(nameof(SoulLink).ToSnakeCase());
    public static readonly DamageSource DamageOverTime = new(nameof(DamageOverTime).ToSnakeCase());

    #endregion

    /// <summary>
    /// Determines whether a modifier of this source can boost damage from source <paramref name="source"/>.
    /// </summary>
    /// <param name="source">Damage source.</param>
    /// <returns>True if a modifier of this source can boost damage from
    /// source <paramref name="source"/>, false otherwise.</returns>
    [System.Diagnostics.Contracts.Pure]
    public bool Permits(DamageSource source) => this == source || this == default;
}