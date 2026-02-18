using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.DamageSystem;

public interface IDamageable
{
    ITargetable Hitbox { get; }
    void TakeDamage(DamageResult dmg);
}