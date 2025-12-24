using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.DamageSystem;
using FrogBattleV4.Core.EffectSystem;

namespace FrogBattleV4.Core.CharacterSystem;

public interface ICharacter : IBattleMember, ISupportsEffects, IHasStats, IHasAbilities, IDamageable
{
    void ExecuteAbility(AbilityExecContext ctx);
}