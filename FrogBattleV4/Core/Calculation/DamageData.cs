namespace FrogBattleV4.Core.Calculation;

public record DamageData(
    DamageType Type = default,
    DamageSource Source = default)
{
    /// <summary>
    /// Determines whether a modifier of this data can boost damage from data <paramref name="data"/>.
    /// </summary>
    /// <param name="data">Damage data.</param>
    /// <returns>True if a modifier of this data can boost damage from
    /// data <paramref name="data"/>, false otherwise.</returns>
    [System.Diagnostics.Contracts.Pure]
    public bool Permits(DamageData data) => Type.Permits(data.Type) && Source.Permits(data.Source);
}

public enum DamageStatChannel
{
    CritRate = 1,
    // Leaves potential for adding things like HitRate and whatnot
}