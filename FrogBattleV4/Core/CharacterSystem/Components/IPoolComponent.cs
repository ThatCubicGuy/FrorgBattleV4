using System;

namespace FrogBattleV4.Core.CharacterSystem.Components;

public interface IPoolComponent
{
    string Id { get; }
    double CurrentValue { get; }
    double? MaxValue { get; }
    double? MinValue { get; }
    PoolFlags Flags { get; }
    /// <summary>
    /// Tries to deduct the amount from the current value. 
    /// </summary>
    /// <param name="amount">Base amount to deduct.</param>
    /// <param name="ctx">The context in which to deduct. Contains owner, ability, and spend mode.</param>
    /// <returns>A <see cref="SpendResult"/> containing the calculated amount
    /// and a boolean indicating whether the current value is sufficient.</returns>
    SpendResult ProcessSpend(double amount, SpendContext ctx);
    /// <summary>
    /// Regenerates an amount to this pool.
    /// </summary>
    /// <param name="amount">Base amount to regen.</param>
    /// <param name="character">The character for which the regen is being done.</param>
    /// <returns>The actual amount being regenerated.</returns>
    double ProcessRegen(double amount, ICharacter character);
}

[Flags] public enum PoolFlags
{
    /// <summary>
    /// If a pool has no flags, then it's probably a "dummy" used by some other abilities.
    /// </summary>
    // That basically makes it a character-wide variable. How did I get to this point lmao
    Dummy,
    UsedForLife = 1 << 0,
    UsedForCasting = 1 << 1,
    UsedForBurst = 1 << 2,
    AbsorbsDamage = 1 << 3,
    AbsorbsHealing = 1 << 4,
    Stuns = 1 << 5,
    
}