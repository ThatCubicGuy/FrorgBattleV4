namespace FrogBattleV4.Core.Pipelines.Pools;

public readonly record struct PoolTag(string Name)
{
    /// <summary>
    /// Name of the pool tag. Automatically converted into snake case.
    /// </summary>
    public string Name { get; } = Name.ToSnakeCase();

    public override string ToString() => Name;

    public static implicit operator PoolTag(string tag) => new(tag);

    #region Common Tags

    public static readonly PoolTag UsedForLife = nameof(UsedForLife);
    public static readonly PoolTag UsedForSpells = nameof(UsedForSpells);
    public static readonly PoolTag UsedForBurst = nameof(UsedForBurst);
    public static readonly PoolTag AbsorbsDamage = nameof(AbsorbsDamage);
    public static readonly PoolTag AbsorbsHealing = nameof(AbsorbsHealing);
    public static readonly PoolTag Stuns = nameof(Stuns);

    #endregion
}