using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using FrogBattleV4.Core.DamageSystem;

namespace FrogBattleV4.Core.AbilitySystem.Components;

public interface IAttackComponent
{
    [Pure]
    [return: NotNull]
    IEnumerable<DamageRequest> GetDamageRequests(AbilityExecContext ctx);
}