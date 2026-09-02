namespace FrogBattleV4.Core.Calculation;

/// <summary>
/// Mutation query classifying a pool mutation (e.g. healing, spending mana).
/// </summary>
public sealed record MutateQuery : DynamicQuery
{
    public required MutateData Data { get; init; }
}