using FrogBattleV4.Core.Combat;

namespace FrogBattleV4.Core.DamageSystem;

public interface IDamageable
{
    ITargetable Hitbox { get; }
    void TakeDamage(DamageResult dmg);
}