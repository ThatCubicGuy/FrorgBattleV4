namespace FrogBattleV4.Core.Calculation;

public readonly record struct DamageType(string Type)
{
    public override string ToString() => Type;

    #region Common Types

    public static readonly DamageType All = default;
    public static readonly DamageType True = new(nameof(True).ToSnakeCase());
    public static readonly DamageType Blunt = new(nameof(Blunt).ToSnakeCase());
    public static readonly DamageType Slash = new(nameof(Slash).ToSnakeCase());
    public static readonly DamageType Pierce = new(nameof(Pierce).ToSnakeCase());
    public static readonly DamageType Bullet = new(nameof(Bullet).ToSnakeCase());
    public static readonly DamageType Blast = new(nameof(Blast).ToSnakeCase());
    public static readonly DamageType Magic = new(nameof(Magic).ToSnakeCase());

    #endregion

    /// <summary>
    /// Determines whether damage with this type can be boosted by a modifier of type <paramref name="modifier"/>.
    /// </summary>
    /// <param name="modifier">Modifier's damage type.</param>
    /// <returns>True if this damage can be boosted by a modifier of
    /// type <paramref name="modifier"/>, false otherwise.</returns>
    public bool Matches(DamageType modifier) =>
        Type == modifier.Type || Type == All.Type || modifier.Type == All.Type;
}