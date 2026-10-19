using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using FrogBattleV4.Core.Calculation;

namespace FrogBattleV4.Core.Modifiers;

/// <summary>
/// Encapsulates an effect that may be applied to a battle member.
/// This effect may have context-dependent stacks. 
/// </summary>
public abstract class ApplicableEffect
{
    [Pure]
    protected abstract int GetStacks(EntityUid subject, EntityUid? reference, BattleEnvironment env);

    [Pure]
    public abstract bool ShouldRemove();

    public abstract void OnTurnStart();
    public abstract void OnTurnEnd();
}