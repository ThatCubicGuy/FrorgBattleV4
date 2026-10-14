using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Combat.Actions;

/// <summary>
/// Initializes a new mutation command with the given properties.
/// </summary>
public class MutateAction : ShardAction<MutateData>
{
    /// <summary>Base amount of the mutation.</summary>
    public double TotalAmount { get; init; }
    public override void Accept(IActionVisitor visitor) => visitor.Visit(this);
}