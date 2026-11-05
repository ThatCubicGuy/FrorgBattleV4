namespace FrogBattleV4.Core.Combat.Commands;

// TODO (Nova): "Data" does not seem to have a consistent meaning across TData parameters.

/// <inheritdoc/>
/// <typeparam name="TData">Data relating to the shard action.</typeparam>
public abstract class BattleCommand<TData> : BattleCommand where TData : notnull
{
    public required TData Data { get; init; }
}

/// <summary>
/// A command causes things to happen in the game. Processed by the battle manager.
/// </summary>
public abstract class BattleCommand
{
    public required RelationContext Relation { get; init; }
    public abstract void Accept(ICommandVisitor visitor);
}