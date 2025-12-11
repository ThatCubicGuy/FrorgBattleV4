using System.Collections.Generic;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.CharacterSystem.Components;
using FrogBattleV4.Core.EffectSystem;

namespace FrogBattleV4.Core.CharacterSystem;

public interface ICharacter : ITargetable, ISupportsEffects, IHasStats, IHasAbilities, ITakesTurn
{
    void ExecuteAbility(AbilityDefinition ability);
}