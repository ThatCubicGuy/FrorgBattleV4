namespace FrogBattleV4.Core.Combat.Actions;

/// <inheritdoc/>
/// <typeparam name="TData">Data relating to the shard action.</typeparam>
public abstract class ShardAction<TData> : ShardAction where TData : notnull
{
    public required TData Data { get; init; }
}

/// <summary>
/// A shard action causes things to happen in the game. Processed by the battle manager.
/// Shard actions are fully resolved.
/// </summary>
public abstract class ShardAction
{
    public required RelationContext Relation { get; init; }
    public abstract void Accept(IActionVisitor visitor);
}