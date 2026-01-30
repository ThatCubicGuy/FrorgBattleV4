#nullable enable
using System;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.DamageSystem;

public struct DamageCalcContext
{
    public ICharacter? Attacker;
    public ICharacter? Target;
    public DamageProperties Properties;
    // It might make sense to forgo DamageProperties, honestly.
    public string? Type;
    public string? Source;
    public Random Rng;
}