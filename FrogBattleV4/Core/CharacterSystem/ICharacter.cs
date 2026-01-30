using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.BattleSystem;
using FrogBattleV4.Core.EffectSystem;

namespace FrogBattleV4.Core.CharacterSystem;

public interface ICharacter : IBattleMember, ISupportsEffects, IHasStats, IHasAbilities
{
    void ExecuteAbility(AbilityExecContext ctx);
}