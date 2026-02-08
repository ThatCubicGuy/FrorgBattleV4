namespace FrogBattleV4.Core.CharacterSystem.Pools;

public readonly record struct PoolTag(string Name)
{
    /// <summary>
    /// Name of the pool tag. Automatically converted into snake case.
    /// </summary>
    public string Name { get; } = Name.ToSnakeCase();

    public override string ToString() => Name;

    public static implicit operator PoolTag(string tag) => new(tag);

    #region Common Tags

    public static readonly PoolTag UsedForLife = new PoolTag(nameof(UsedForLife));
    public static readonly PoolTag UsedForSpells = new PoolTag(nameof(UsedForSpells));
    public static readonly PoolTag UsedForBurst = new PoolTag(nameof(UsedForBurst));
    public static readonly PoolTag AbsorbsDamage = new PoolTag(nameof(AbsorbsDamage));
    public static readonly PoolTag AbsorbsHealing = new PoolTag(nameof(AbsorbsHealing));

    #endregion
}