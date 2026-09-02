namespace FrogBattleV4.Core.Calculation;

public readonly record struct DamageType(string Type)
{
    /// <summary>
    /// Unique ID of the damage type. Automatically converted into snake case.
    /// </summary>
    public string Type { get; } = Type.ToSnakeCase();

    public override string ToString() => Type;

    public static implicit operator DamageType(string type) => new(type);

    #region Common Types

    public static readonly DamageType All = default;
    public static readonly DamageType True = nameof(True);
    public static readonly DamageType Blunt = nameof(Blunt);
    public static readonly DamageType Slash = nameof(Slash);
    public static readonly DamageType Pierce = nameof(Pierce);
    public static readonly DamageType Bullet = nameof(Bullet);
    public static readonly DamageType Blast = nameof(Blast);
    public static readonly DamageType Magic = nameof(Magic);

    #endregion

    public bool Matches(DamageType other) =>
        Type == other.Type || Type == All.Type || other.Type == All.Type;
}