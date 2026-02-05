#nullable enable
using System;
using System.Collections.Generic;
using FrogBattleV4.Core.EffectSystem.StatusEffects;

namespace FrogBattleV4.Core.CharacterSystem.Pools;

public interface IPoolComponent
{
    string Id { get; }
    double CurrentValue { get; set; }
    double? MaxValue { get; }
    double? MinValue { get; }
    PoolFlags Flags { get; }
    IEnumerable<IMutatorComponent>? Mutators { get; }
}

/// <summary>
/// Represents flags that may be attached to a pool, to state its functionalities.
/// </summary>
[Flags] public enum PoolFlags
{
    /// <summary>
    /// If a pool has no flags, then it's probably a "dummy" used by some other abilities.
    /// </summary>
    Dummy,
    // ^ That basically makes it a character-wide variable. How did I get to this point lmao
    UsedForLife = 1 << 0,
    UsedForCasting = 1 << 1,
    UsedForBurst = 1 << 2,
    AbsorbsDamage = 1 << 3,
    AbsorbsHealing = 1 << 4,
    Stuns = 1 << 5
}