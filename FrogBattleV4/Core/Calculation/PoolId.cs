namespace FrogBattleV4.Core.Calculation;

public readonly record struct PoolId(string Id)
{
    public override string ToString() => Id;

    #region Common Pools

    public static readonly PoolId Hp = new(nameof(Hp).ToSnakeCase());
    public static readonly PoolId Mana = new(nameof(Mana).ToSnakeCase());
    public static readonly PoolId Energy = new(nameof(Energy).ToSnakeCase());
    public static readonly PoolId Shield = new(nameof(Shield).ToSnakeCase());
    public static readonly PoolId Barrier = new(nameof(Barrier).ToSnakeCase());
    public static readonly PoolId Overheal = new(nameof(Overheal).ToSnakeCase());

    #endregion
}