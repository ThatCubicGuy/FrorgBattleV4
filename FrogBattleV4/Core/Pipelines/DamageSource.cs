using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.Pipelines;

public readonly record struct DamageSource([NotNull] string Source)
{
    /// <summary>
    /// Unique ID of the damage type. Automatically converted into snake case.
    /// </summary>
    [NotNull] public string Source { get; private init; } = Source.ToSnakeCase();

    public override string ToString() => Source;

    public static implicit operator DamageSource([NotNull] string source) => new(source);

    #region Common Sources

    public static readonly DamageSource None = new() { Source = null! };
    public static readonly DamageSource Ability = nameof(Ability);
    public static readonly DamageSource FollowUp = nameof(FollowUp);
    public static readonly DamageSource Ultimate = nameof(Ultimate);
    public static readonly DamageSource DamageOverTime = nameof(DamageOverTime);

    #endregion

    public bool Matches(DamageSource other) => this == None || other == None || this == other;
}