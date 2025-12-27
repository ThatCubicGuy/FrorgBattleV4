using System.Collections.Generic;
using FrogBattleV4.Core.AbilitySystem;
using FrogBattleV4.Core.CharacterSystem;

namespace FrogBattleV4.Core.BattleSystem.Decisions;

public record AbilityDecisionRequester(ICharacter Owner) : IDecisionRequester<AbilityDefinition>
{
    public IEnumerable<AbilityDefinition> GetOptions(BattleContext ctx) => Owner.Abilities;
}