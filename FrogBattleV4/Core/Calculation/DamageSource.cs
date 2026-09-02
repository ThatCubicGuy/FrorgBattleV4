namespace FrogBattleV4.Core.Calculation;

public readonly record struct DamageSource(string Source)
{
    /// <summary>
    /// Unique ID of the damage type. Automatically converted into snake case.
    /// </summary>
    public string Source { get; } = Source.ToSnakeCase();

    public override string ToString() => Source;

    public static implicit operator DamageSource(string source) => new(source);

    #region Common Sources

    public static readonly DamageSource All = default;
    public static readonly DamageSource Link = nameof(Link);
    public static readonly DamageSource FollowUp = nameof(FollowUp);
    public static readonly DamageSource Soullink = nameof(Soullink);
    public static readonly DamageSource DamageOverTime = nameof(DamageOverTime);

    #endregion

    public bool Matches(DamageSource other) =>
        Source == other.Source || Source == All.Source || other.Source == All.Source;
}