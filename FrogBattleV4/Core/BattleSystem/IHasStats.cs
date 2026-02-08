#nullable enable

namespace FrogBattleV4.Core.BattleSystem;

public interface IHasStats
{
    /// <summary>
    /// Calculates the final value of a stat, optionally in relation to an enemy.
    /// </summary>
    /// <param name="stat">The name of the stat to calculate.</param>
    /// <param name="target">The enemy against which to calculate the stat. Optional.</param>
    /// <returns>The final value of the stat.</returns>
    double GetStat(string stat, BattleMember? target = null);
}