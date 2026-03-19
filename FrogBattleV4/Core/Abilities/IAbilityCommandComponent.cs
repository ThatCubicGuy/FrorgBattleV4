using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace FrogBattleV4.Core.Abilities;

public interface IAbilityCommandComponent : IAbilityComponent
{
    [Pure]
    IEnumerable<IBattleCommand> GetContribution(AbilityExecContext ctx);
}