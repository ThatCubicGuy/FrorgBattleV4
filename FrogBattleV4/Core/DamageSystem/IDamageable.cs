using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.CharacterSystem.Pools;

namespace FrogBattleV4.Core.DamageSystem;

public interface IDamageable
{
    PoolComponent Hp { get; }
    ITargetable Hitbox { get; }
}