using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Combat.Commands;

/// <summary>
/// Initializes a new mutation command with the given properties.
/// </summary>
public class MutateCommand : BattleCommand
{
    /// <summary>Base amount of the mutation.</summary>
    public double TotalAmount { get; init; }
    public required PoolName TargetPool { get; init; }
    public required PoolMutChannel Channel { get; init; }
    public override void Accept(ICommandVisitor visitor) => visitor.Visit(this);
}