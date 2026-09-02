namespace FrogBattleV4.Core.Combat.Actions;

public abstract class ShardAction<TData> : ShardAction where TData : notnull
{
    public required TData Data { get; init; }
}

public abstract class ShardAction
{
    public required RelationContext Relation { get; init; }
    public abstract void Accept(IActionVisitor visitor);
}