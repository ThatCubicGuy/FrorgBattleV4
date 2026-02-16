using System.Diagnostics.CodeAnalysis;

namespace FrogBattleV4.Core.Pipelines;

public readonly record struct DamageType([NotNull] string Type)
{
    /// <summary>
    /// Unique ID of the damage type. Automatically converted into snake case.
    /// </summary>
    [NotNull] public string Type { get; private init; } = Type.ToSnakeCase();

    public override string ToString() => Type;

    public static implicit operator DamageType([NotNull] string type) => new(type);

    #region Common Types

    public static readonly DamageType None = new() { Type = null! };
    public static readonly DamageType True = nameof(True);
    public static readonly DamageType Blunt = nameof(Blunt);
    public static readonly DamageType Slash = nameof(Slash);
    public static readonly DamageType Pierce = nameof(Pierce);
    public static readonly DamageType Bullet = nameof(Bullet);
    public static readonly DamageType Blast = nameof(Blast);
    public static readonly DamageType Magic = nameof(Magic);

    #endregion

    public bool Matches(DamageType other) => this == None || other == None || this == other;
}