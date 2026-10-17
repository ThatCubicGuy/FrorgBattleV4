namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Command query classifying damage.
/// </summary>
public class DamageQuery : CommandQuery
{
    public required DamageData Data { get; init; }
    public required bool IsCrit { get; init; }
}

public class DamageStatQuery : AttributeQuery
{
    public required DamageData Data { get; init; }
    public required DamageStatChannel Channel { get; init; }
}