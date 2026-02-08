using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.DamageSystem;

public interface IDamageable
{
    void ReceiveDamage(DamageResult dmg);
    void ReceiveHealing(double healing);

    /// <summary>
    /// The actual battle member whom this targetable entity refers to.
    /// </summary>
    BattleMember Parent { get; }
}