using System.Text.Json.Serialization;
using FrogBattleV4.JsonDeserialization;

namespace FrogBattleV4.Core.Calculation;

[JsonConverter(typeof(ExpandableEnumConverter<DamageSource>))]
public readonly record struct DamageSource(string Identifier) : IExpandableEnum
{
    public override string ToString() => Identifier;

    #region Common Sources

    public static readonly DamageSource All = default;
    public static readonly DamageSource Link = new(nameof(Link).ToSnakeCase());
    public static readonly DamageSource FollowUp = new(nameof(FollowUp).ToSnakeCase());
    public static readonly DamageSource SoulLink = new(nameof(SoulLink).ToSnakeCase());
    public static readonly DamageSource DamageOverTime = new(nameof(DamageOverTime).ToSnakeCase());

    #endregion

    /// <summary>
    /// Determines whether damage from this source can be boosted by a modifier of source <paramref name="modifier"/>.
    /// </summary>
    /// <param name="modifier">Modifier's damage source.</param>
    /// <returns>True if this damage can be boosted by a modifier of
    /// source <paramref name="modifier"/>, false otherwise.</returns>
    public bool Matches(DamageSource modifier) =>
        Identifier == modifier.Identifier || Identifier == All.Identifier || modifier.Identifier == All.Identifier;
}