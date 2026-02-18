namespace FrogBattleV4.Core.Calculation;

public readonly record struct PoolId(string Id)
{
    /// <summary>
    /// Unique ID of the pool. Automatically converted into snake case.
    /// </summary>
    public string Id { get; } = Id.ToSnakeCase();

    public override string ToString() => Id;

    public static implicit operator PoolId(string id) => new(id);

    #region Common Pools

    public static readonly PoolId Hp = nameof(Hp);
    public static readonly PoolId Mana = nameof(Mana);
    public static readonly PoolId Energy = nameof(Energy);
    public static readonly PoolId Shield = nameof(Shield);
    public static readonly PoolId Barrier = nameof(Barrier);
    public static readonly PoolId Special = nameof(Special);

    #endregion
}