namespace FrogBattleV4.Core.Combat.Commands;

/// <summary>
/// A command causes things to happen in the game. Processed by the battle manager.
/// </summary>
public abstract class BattleCommand
{
    public required RelationContext Relation { get; init; }
    public abstract void Accept(ICommandVisitor visitor);
}