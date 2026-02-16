#nullable enable
namespace FrogBattleV4.Core.Pipelines;

public readonly record struct StatId(string Id)
{
    /// <summary>
    /// Unique ID of the stat. Automatically converted into snake case.
    /// </summary>
    public string Id { get; } = Id.ToSnakeCase();

    public override string ToString() => Id;

    public static implicit operator StatId(string id) => new(id);

    #region Common Stats

    public static readonly StatId MaxHp = nameof(MaxHp);
    public static readonly StatId MaxMana = nameof(MaxMana);
    public static readonly StatId MaxEnergy = nameof(MaxEnergy); // Positive = Bad
    public static readonly StatId Atk = nameof(Atk);
    public static readonly StatId Def = nameof(Def);
    public static readonly StatId Spd = nameof(Spd);
    public static readonly StatId Dex = nameof(Dex);
    public static readonly StatId CritRate = nameof(CritRate);
    public static readonly StatId CritDamage = nameof(CritDamage);
    public static readonly StatId HitRateBonus = nameof(HitRateBonus);
    public static readonly StatId EffectHitRate = nameof(EffectHitRate);
    public static readonly StatId EffectRes = nameof(EffectRes);
    public static readonly StatId ManaCost = nameof(ManaCost); // Positive = Bad
    public static readonly StatId ManaRegen = nameof(ManaRegen);
    public static readonly StatId EnergyRecharge = nameof(EnergyRecharge);
    public static readonly StatId IncomingHealing = nameof(IncomingHealing);
    public static readonly StatId OutgoingHealing = nameof(OutgoingHealing);
    public static readonly StatId ShieldToughness = nameof(ShieldToughness);

    #endregion
}