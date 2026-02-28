using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.AbilitySystem;

public interface IAbilityCommandComponent : IAbilityComponent
{
    [Pure]
    IEnumerable<IBattleCommand> GetContribution(AbilityExecContext ctx);
}