using FrogBattleV4.Core.Abilities.Components;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Combat.Actions;

/// <summary>
/// Initializes a new mutation command with the given properties.
/// </summary>
public class Mutate : ShardAction<MutateData>
{
    /// <summary>Base amount of the mutation.</summary>
    public double TotalAmount { get; init; }
    /// <summary>The ID of the pool to mutate.</summary>
    public override void Accept(IActionVisitor visitor) => visitor.Visit(this);
}