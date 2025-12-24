using System;

namespace FrogBattleV4.Core.CharacterSystem.Components;

public interface IPoolComponent
{
    string Id { get; }
    double CurrentValue { get; set; }
    /// <summary>
    /// The maximum value that this pool component can have.
    /// </summary>
    double? MaxValue { get; }
    PoolFlags Flags { get; }
}

[Flags]
public enum PoolFlags
{
    // If a pool has no flags, then it's probably a "dummy" used by some other abilities.
    // That basically makes it a character-wide variable. How did I get to this point lmao
    Dummy,
    UsedForCasting = 1 << 0,
    UsedForBurst = 1 << 1,
    UsedForLife = 1 << 2,
    AbsorbsDamage = 1 << 3,
    AbsorbsHealing = 1 << 4,
    Stuns = 1 << 5,
    
}