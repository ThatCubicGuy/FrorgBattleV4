namespace FrogBattleV4.Core.Calculation;

[System.Flags] public enum HealData
{
    None = 0,
    Overheals = 1 << 0,
    Revives = 1 << 1,
}