using System;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.DamageSystem;

public struct DamageContext
{
    public double RawDamage;
    public ICharacter Attacker;
    public ICharacter Target;
    public DamageProperties Properties;
    /* It might make sense to forgo DamageProperties, honestly.
     * public string Type;
     * public double DefIgnore;
     * public double TypeResPen;
     */
    public string DamageSource;
    public Random Rng;
}