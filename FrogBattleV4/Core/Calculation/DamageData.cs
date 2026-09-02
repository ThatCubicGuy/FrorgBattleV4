namespace FrogBattleV4.Core.Calculation;

public record DamageData(
    DamageType Type,
    DamageSource Source);

public enum DamageStatChannel
{
    CritRate = 1,
    // Leaves potential for adding things like HitRate and whatnot
}

public record CritResolution(double Chance, double Roll)
{
    public CritStatus Outcome => Chance > Roll ? CritStatus.Critical : CritStatus.Normal;
}

public enum CritStatus
{
    Normal,
    Critical
}