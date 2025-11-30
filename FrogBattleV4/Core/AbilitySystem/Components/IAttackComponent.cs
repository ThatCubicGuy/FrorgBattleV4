using System.Collections.Generic;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components;

public interface IAttackComponent : IAbilityComponent
{
    IEnumerable<Damage> GetDamage(AbilityContext ctx);
}