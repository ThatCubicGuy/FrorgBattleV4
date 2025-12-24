namespace FrogBattleV4.Core.DamageSystem;

public interface IDamageable
{
    void TakeDamage(DamageCalcContext ctx);
}