using System.Collections.Generic;
using FrogBattleV4.Core.BattleSystem;

namespace FrogBattleV4.Core.AbilitySystem;

public interface IHasAbilities : IBattleMember
{
    IEnumerable<AbilityDefinition> Abilities { get; }
}