namespace FrogBattleV4.Core.CharacterSystem.Pools;

public readonly record struct PoolId(string Id)
{
    /// <summary>
    /// Unique ID of the pool. Automatically converted into snake case.
    /// </summary>
    public string Id { get; } = Id.ToSnakeCase();

    public override string ToString() => Id;

    public static implicit operator PoolId(string id) => new(id);
}