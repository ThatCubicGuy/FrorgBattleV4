namespace FrogBattleV4.Core.DamageSystem;

/// <summary>
/// A fully calculated instance of damage. The raw value of <paramref name="Amount"/>
/// is deducted from the HP of the target.<br/>This record is mostly used for displays.
/// </summary>
/// <param name="Amount">The amount of damage taken.</param>
/// <param name="Type">The type of the damage dealt.</param>
/// <param name="IsCrit">Whether this damage instance is a critical hit.</param>
public record DamageSnapshot(double Amount, string Type, bool IsCrit);