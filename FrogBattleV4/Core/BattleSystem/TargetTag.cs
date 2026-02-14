namespace FrogBattleV4.Core.BattleSystem;

public readonly record struct TargetTag(string Name)
{
    /// <summary>
    /// Name of the pool tag. Automatically converted into snake case.
    /// </summary>
    public string Name { get; } = Name.ToSnakeCase();

    public override string ToString() => Name;

    public static implicit operator TargetTag(string tag) => new(tag);

    #region Common Tags

    public static readonly TargetTag WeakPoint = nameof(WeakPoint);
    public static readonly TargetTag MainBody = nameof(MainBody);
    public static readonly TargetTag Limb = nameof(Limb);

    #endregion
}