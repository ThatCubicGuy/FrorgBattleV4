namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Mutation query classifying damage.
/// </summary>
public sealed record DamageQuery : DynamicQuery
{
    public required DamageData Data { get; init; }
    public required CritResolution CritData { get; init; }
    public bool IsCrit => CritData.Outcome == CritStatus.Critical;
}

public sealed record DamageStatQuery : StaticQuery
{
    public required DamageData Data { get; init; }
    public required DamageStatChannel Channel { get; init; }
}