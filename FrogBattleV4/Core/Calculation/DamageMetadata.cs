namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Metadata that describes a kind of damage.
/// Does not directly affect what this damage does, but
/// modifiers can apply to specific kinds of damage.
/// </summary>
/// <param name="Form">Form of the damage (blunt, slash, pierce, bullet, blast, magic...).</param>
/// <param name="Source">Source of the damage (link, dot, follow-up, soul-link...)</param>
public record DamageMetadata(
    DamageForm Form = default,
    DamageSource Source = default)
{
    public static readonly DamageMetadata Default = new();

    /// <summary>
    /// Determines whether a modifier of this metadata can boost damage from metadata <paramref name="metadata"/>.
    /// </summary>
    /// <param name="metadata">Damage metadata.</param>
    /// <returns>True if a modifier of this metadata can boost damage from
    /// data <paramref name="metadata"/>, false otherwise.</returns>
    [System.Diagnostics.Contracts.Pure]
    public bool Permits(DamageMetadata metadata) => Form.Permits(metadata.Form) && Source.Permits(metadata.Source);
}