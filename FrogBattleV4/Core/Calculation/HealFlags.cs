namespace FrogBattleV4.Core.Calculation;

[System.Flags] public enum HealFlags
{
    None = 0,
    Overheals = 1 << 0,
    Revives = 1 << 1,
}