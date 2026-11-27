namespace FrogBattleV4.Core.Calculation;

[System.Flags] public enum DamageFlags
{
    None = 0,
    IgnoresBarrier = 1 << 0,
}