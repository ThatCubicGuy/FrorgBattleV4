namespace FrogBattleV4.Core.DamageSystem;

public interface IDamageable
{
    void TakeDamage(DamageSnapshot dmg);
    void TakeHealing(double healing);
}