namespace FrogBattleV4.Core.Calculation;

public readonly record struct DamageSource(string Source)
{
    /// <summary>
    /// Unique ID of the damage type. Automatically converted into snake case.
    /// </summary>
    public string Source { get; } = Source.ToSnakeCase();

    public override string ToString() => Source;

    #region Common Sources

    public static readonly DamageSource All = default;
    public static readonly DamageSource Link = new(nameof(Link).ToSnakeCase());
    public static readonly DamageSource FollowUp = new(nameof(FollowUp).ToSnakeCase());
    public static readonly DamageSource SoulLink = new(nameof(SoulLink).ToSnakeCase());
    public static readonly DamageSource DamageOverTime = new(nameof(DamageOverTime).ToSnakeCase());

    #endregion

    public bool Matches(DamageSource other) =>
        Source == other.Source || Source == All.Source || other.Source == All.Source;
}