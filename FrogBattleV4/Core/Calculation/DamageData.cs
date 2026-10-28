namespace FrogBattleV4.Core.Calculation;

public record DamageData(
    DamageType Type,
    DamageSource Source)
{
    /// <summary>
    /// Determines whether damage with this data can be boosted by a modifier of data <paramref name="modifier"/>.
    /// </summary>
    /// <param name="modifier">Modifier's damage data.</param>
    /// <returns>True if this damage can be boosted by a modifier of
    /// data <paramref name="modifier"/>, false otherwise.</returns>
    [System.Diagnostics.Contracts.Pure]
    public bool Matches(DamageData modifier)
    {
        return Type.Matches(modifier.Type) && Source.Matches(modifier.Source);
    }
}

public enum DamageStatChannel
{
    CritRate = 1,
    // Leaves potential for adding things like HitRate and whatnot
}